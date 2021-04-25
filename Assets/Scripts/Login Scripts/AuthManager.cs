using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace WorldWarOneTools
{
    public class AuthManager : MonoBehaviour
    {
        public static AuthManager instance;

        [Header("Firebase")]
        public FirebaseAuth auth;
        public FirebaseUser user;
        public DatabaseReference databaseReference;
        [Space(5f)]

        [Header("Login Reference")]
        [SerializeField]
        private InputField loginEmail;
        [SerializeField]
        private InputField loginPassword;
        [SerializeField]
        private TMP_Text loginOutputText;
        [Space(5f)]

        [Header("Register Reference")]
        [SerializeField]
        private InputField registerUsername;
        [SerializeField]
        private InputField registerEmail;
        [SerializeField]
        private InputField registerPassword;
        [SerializeField]
        private InputField registerConfirmPassword;
        [SerializeField]
        private TMP_Text registerOutputText;

        public SceneReference sceneReference;

        [SerializeField] private UIManager uiManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(instance.gameObject);
                instance = this;
            }
        }

        private void Start()
        {
            StartCoroutine(CheckAndFixDependencies());
        }
        private IEnumerator CheckAndFixDependencies()
        {
            var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

            yield return new WaitUntil(predicate: () => checkAndFixDependenciesTask.IsCompleted);

            var dependancyResult = checkAndFixDependenciesTask.Result;

            if (dependancyResult == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependancy: {dependancyResult}");
            }
        }

        private void InitializeFirebase()
        {
            auth = FirebaseAuth.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            StartCoroutine(CheckAutoLogin());

            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);

        }
        private IEnumerator CheckAutoLogin()
        {
            yield return new WaitForEndOfFrame();
            if (user != null)
            {
                var reloadUserTask = user.ReloadAsync();

                yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);

                AutoLogin();
            }
            else
            {
                uiManager.LoginScreen();
            }
        }

        private void AutoLogin()
        {
            if (user != null)
            {
                if (user.IsEmailVerified)
                {
                    SceneManager.LoadScene(sceneReference);
                }
                else
                {
                    StartCoroutine(SendVerificationEmail());
                }
            }
            else
            {
                uiManager.LoginScreen();
            }
        }

        private void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            if (auth.CurrentUser != user)
            {
                bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

                if (!signedIn && user != null)
                {
                    Debug.Log("Sign Out");
                }

                user = auth.CurrentUser;

                if (signedIn)
                {
                    Debug.Log($"Signed In: { user.DisplayName}");
                }
            }
        }

        public void ClearOutputs()
        {
            loginOutputText.text = "";
            registerOutputText.text = "";
        }

        public void LoginButton()
        {
            StartCoroutine(LoginLogic(loginEmail.text, loginPassword.text));
        }

        public void RegisterButton()
        {
            StartCoroutine(RegisterLogic(registerUsername.text, registerEmail.text, registerPassword.text, registerConfirmPassword.text));
        }

        private IEnumerator LoginLogic(string _email, string _password)
        {
            Credential credential = EmailAuthProvider.GetCredential(_email, _password);

            var loginTask = auth.SignInWithCredentialAsync(credential);

            yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;
                string output = "Unknown Error, Please Try again";

                switch (error)
                {
                    case AuthError.MissingEmail:
                        output = "Please Enter Your Email";
                        break;
                    case AuthError.MissingPassword:
                        output = "Please Enter Your Password";
                        break;
                    case AuthError.InvalidEmail:
                        output = "Invalid Email";
                        break;
                    case AuthError.WrongPassword:
                        output = "Incorrect Password";
                        break;
                    case AuthError.UserNotFound:
                        output = "Account Does not Exist";
                        break;
                }
                loginOutputText.text = output;
            }
            else
            {
                if (user.IsEmailVerified)
                {
                    yield return new WaitForSeconds(1f);
                    SceneManager.LoadScene(sceneReference);
                }
                else
                {
                    StartCoroutine(SendVerificationEmail());
                }
            }

        }

        private IEnumerator RegisterLogic(string _username, string _email, string _password, string _confirmPassword)
        {
            if (_username == "")
            {
                registerOutputText.text = "Please Enter A Username";
            }
            else if (_password != _confirmPassword)
            {
                registerOutputText.text = "Passwords Do not Match!";
            }
            else
            {
                var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

                yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

                if (registerTask.Exception != null)
                {
                    FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;
                    string output = "Unknown Error, Please Try again";

                    switch (error)
                    {
                        case AuthError.InvalidEmail:
                            output = "Invalid Email";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            output = "Email Already in Use";
                            break;
                        case AuthError.WeakPassword:
                            output = "Weak Password";
                            break;
                        case AuthError.MissingEmail:
                            output = "Please Enter Your Email";
                            break;
                        case AuthError.MissingPassword:
                            output = "Please Enter Your Password";
                            break;
                    }
                    registerOutputText.text = output;
                }
                else
                {
                    UserProfile profile = new UserProfile
                    {
                        DisplayName = _username,
                    };

                    var defaultUserTask = user.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);

                    if (defaultUserTask.Exception != null)
                    {
                        user.DeleteAsync();
                        FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
                        AuthError error = (AuthError)firebaseException.ErrorCode;
                        string output = "Unknown Error, Please Try again";

                        switch (error)
                        {
                            case AuthError.Cancelled:
                                output = "Update User Cancelled";
                                break;
                            case AuthError.SessionExpired:
                                output = "Session Expired";
                                break;
                        }
                        registerOutputText.text = output;
                    }
                    else
                    {
                        Debug.Log($"Firebase User Created Succuessfully: {user.DisplayName} ({ user.UserId})");

                        StartCoroutine(SendVerificationEmail());
                    }
                }
            }
        }

        private IEnumerator SendVerificationEmail()
        {
            if (user != null)
            {
                var emailTask = user.SendEmailVerificationAsync();

                yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

                if (emailTask.Exception != null)
                {
                    FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;

                    string output = "Unknown Error";

                    switch (error)
                    {
                        case AuthError.Cancelled:
                            output = "Verification was Cancelled";
                            break;
                        case AuthError.InvalidRecipientEmail:
                            output = "Invalid Email";
                            break;
                        case AuthError.TooManyRequests:
                            output = "Too Many Request";
                            break;
                    }

                    uiManager.AwaitVerification(false, user.Email, output);
                }
                else
                {
                    uiManager.AwaitVerification(true, user.Email, null);
                    Debug.Log("Email Sent Successfully");
                }
            }
        }

        public IEnumerator SaveGameFile(int gamefile)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("GameFile").SetValueAsync(gamefile);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator SaveScene(string scene)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("CurrentScene").SetValueAsync(scene);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator SaveSpawnReference(int spawnReference)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("SpawnReference").SetValueAsync(spawnReference);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator SpawnReference(int mainSaveSpawnReference)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("MainSaveSpawnReference").SetValueAsync(mainSaveSpawnReference);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator CurrentHealth(int currentHealth)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("CurrentHealth").SetValueAsync(currentHealth);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator FacingLeft(int direction)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("Facing").SetValueAsync(direction);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }
        public IEnumerator LoadFormSave(int save)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("LoadFormSave").SetValueAsync(save);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator Character(int selectedCharacter)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("Character").SetValueAsync(selectedCharacter);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator Mission(int mission)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("Mission").SetValueAsync(mission);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator Checkpoint(int checkpoint)
        {
            var DBTask = databaseReference.Child("users").Child(user.UserId).Child("Checkpoint").SetValueAsync(checkpoint);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        public IEnumerator LoadUserData()
        {
            //Get the currently logged in user data
            var DBTask = databaseReference.Child("users").Child(user.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                //No data exists yet
            }
            else
            {
                //Data has been retrieved
                DataSnapshot snapshot = DBTask.Result;

                PlayerPrefs.SetInt("GameFile", (int) (long)snapshot.Child("GameFile").Value);
                PlayerPrefs.SetString(" " + 1 + "LoadGame", snapshot.Child("CurrentScene").Value.ToString());
                PlayerPrefs.SetInt(" " + 1 + "SaveSpawnReference", (int) (long)snapshot.Child("SpawnReference").Value);
                PlayerPrefs.SetInt(" " + 1 + "SpawnReference", (int) (long)snapshot.Child("MainSaveSpawnReference").Value);
                PlayerPrefs.SetInt(" " + 1 + "CurrentHealth", (int) (long)snapshot.Child("CurrentHealth").Value);
                PlayerPrefs.SetInt(" " + 1 + "FacingLeft", (int) (long)snapshot.Child("Facing").Value);
                PlayerPrefs.SetInt(" " + 1 + "LoadFromSave", (int) (long)snapshot.Child("LoadFormSave").Value);
                PlayerPrefs.SetInt("char", (int) (long)snapshot.Child("Character").Value);
                PlayerPrefs.SetInt("Mission", (int) (long)snapshot.Child("Mission").Value);

            }
        }
    }
}
