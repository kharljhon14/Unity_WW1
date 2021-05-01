using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace WorldWarOneTools
{
    public class MainMenuManager : MonoBehaviour
    {

        public SceneReference newGameScene;
        public SceneReference missionSelect;

        public List<AbiltyItem> abilitiesToClear = new List<AbiltyItem>();

        [SerializeField] private TextMeshProUGUI userName;
        [SerializeField] private Image fadeScreen;
        [SerializeField] private float fadeSpeed;
        [SerializeField] private GameObject aboutUs;

        private AuthManager authManager;

        private void Awake()
        {
            authManager = FindObjectOfType<AuthManager>();
            userName.text = "Welcome " + authManager.user.DisplayName;
        }

        private void Start()
        {
            StartCoroutine(FadeIn());
            StartCoroutine(authManager.LoadUserData());
        }

        public virtual void NewGame(int slot)
        {
            AudioManager.instance.PlaySFX(3);
            PlayerPrefs.SetInt("GameFile", slot);
            PlayerPrefs.SetInt(" " + slot + "SaveSpawnReference", 0);
            PlayerPrefs.SetInt(" " + slot + "SpawnReference", 0);
            PlayerPrefs.SetInt(" " + slot + "CurrentHealth", 100);
            PlayerPrefs.SetString("LoadGame", newGameScene);
            DeleteCheckpointKeys();
            ClearAbilities(slot);

            StartCoroutine(authManager.SaveGameFile(PlayerPrefs.GetInt("GameFile")));
            StartCoroutine(authManager.SaveScene(PlayerPrefs.GetString("LoadGame")));
            StartCoroutine(authManager.SaveSpawnReference((PlayerPrefs.GetInt(" " + slot + "SaveSpawnReference"))));
            StartCoroutine(authManager.SpawnReference(PlayerPrefs.GetInt(" " + slot + "SpawnReference")));
            StartCoroutine(authManager.CurrentHealth(PlayerPrefs.GetInt(" " + slot + "CurrentHealth")));

            StartCoroutine(FadeOut(newGameScene));
        }

        public virtual void LoadGame(int slot)
        {
            bool loadFromSave = true;
            PlayerPrefs.SetInt("GameFile", slot);
            PlayerPrefs.SetInt(" " + slot + "LoadFromSave", loadFromSave ? 1 : 0);

            StartCoroutine(authManager.SaveGameFile(PlayerPrefs.GetInt("GameFile")));
            StartCoroutine(authManager.LoadFormSave(PlayerPrefs.GetInt(" " + slot + "LoadFromSave")));

            StartCoroutine(FadeOutContinue(PlayerPrefs.GetString(" " + slot + "LoadGame")));
        }

        public virtual void MissionSelect()
        {
            if(authManager.databaseReference != null)
            {
                AudioManager.instance.PlaySFX(3);
                StartCoroutine(FadeOut(missionSelect));
            }
               
        }

        public virtual void ClearAbilities(int slot)
        {
            for (int i = 0; i < abilitiesToClear.Count; i++)
            {
                PlayerPrefs.SetInt(" " + slot + abilitiesToClear[i].itemName, 0);
            }
        }

        public void OpenAboutUs()
        {
            AudioManager.instance.PlaySFX(3);
            aboutUs.SetActive(true);
        }

        public void CloseAboutUs()
        {
            AudioManager.instance.PlaySFX(3);
            aboutUs.SetActive(false);
        }

        public virtual void SignOut()
        {
            AudioManager.instance.PlaySFX(3);
            authManager.auth.SignOut();
            Application.Quit();
        }

        protected virtual void DeleteCheckpointKeys()
        {
            PlayerPrefs.DeleteKey("Unlock");
            PlayerPrefs.DeleteKey("Mission");
        }

        protected virtual IEnumerator FadeIn()
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / fadeSpeed;
            Color currentColor = fadeScreen.color;

            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / fadeSpeed;
                currentColor.a = Mathf.Lerp(1, 0, percentageComplete);

                fadeScreen.color = currentColor;

                if (percentageComplete >= 1)
                {
                    fadeScreen.raycastTarget = false;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        protected virtual IEnumerator FadeOut(SceneReference scene)
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / fadeSpeed;
            Color currentColor = fadeScreen.color;

            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / fadeSpeed;
                currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
                fadeScreen.color = currentColor;
                fadeScreen.raycastTarget = true;
                if (percentageComplete >= 1)
                    break;

                yield return new WaitForEndOfFrame();
            }

            SceneManager.LoadScene(scene);
        }

        protected virtual IEnumerator FadeOutContinue(string scene)
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / fadeSpeed;
            Color currentColor = fadeScreen.color;

            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / fadeSpeed;
                currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
                fadeScreen.color = currentColor;
                fadeScreen.raycastTarget = true;
                if (percentageComplete >= 1)
                    break;

                yield return new WaitForEndOfFrame();
            }

            SceneManager.LoadScene(scene);
        }
    }
}
