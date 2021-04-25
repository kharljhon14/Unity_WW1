using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace WorldWarOneTools
{
    public class LevelManager : Manager
    {
        public Bounds levelSize;
        public GameObject[] initialPlayer;
        public Image fadeScreen;

        [SerializeField] private TextMeshProUGUI dateText;
        [SerializeField] private int stageToUnlock;
        [SerializeField] private int checkpointToUnlock;
        [SerializeField] private float fadeSpeed;
        [SerializeField] private GameObject soulImg;

        public List<Transform> availableSpawnLocations = new List<Transform>();

        private Vector3 startingLocation;
        private AuthManager authManager;

        [HideInInspector] public bool loadFromSave;

        protected virtual void Awake()
        {
            int gameFile = PlayerPrefs.GetInt("GameFile");
            loadFromSave = PlayerPrefs.GetInt(" " + gameFile + "LoadFromSave") == 1 ? true : false;

            if (loadFromSave)
            {
                startingLocation = availableSpawnLocations[PlayerPrefs.GetInt(" " + gameFile + "SaveSpawnReference")].position;
            }

            if (availableSpawnLocations.Count <= PlayerPrefs.GetInt(" " + gameFile + "SpawnReference"))
            {
                startingLocation = availableSpawnLocations[0].position; 
            }

            else
            {
                if (!loadFromSave)
                {
                    startingLocation = availableSpawnLocations[PlayerPrefs.GetInt(" " + gameFile + "SpawnReference")].position;
                }

                if (PlayerPrefs.GetInt("char") == 0)
                    CreatePlayer(initialPlayer[0], startingLocation);

                else if(PlayerPrefs.GetInt("char") == 1)
                    CreatePlayer(initialPlayer[1], startingLocation);

                else
                    CreatePlayer(initialPlayer[2], startingLocation);
            }     
        }

        protected override void Initialization()
        {
            base.Initialization();

            authManager = FindObjectOfType<AuthManager>();
            int gameFile = PlayerPrefs.GetInt("GameFile");
            loadFromSave = false;

            PlayerPrefs.SetInt(" " + gameFile + "LoadFromSave", levelManager.loadFromSave ? 1 : 0);
            PlayerPrefs.SetInt(" " + character.gameFile + "FacingLeft", character.isFacingLeft ? 1 : 0);
            PlayerPrefs.SetString(" " + character.gameFile + "LoadGame", SceneManager.GetActiveScene().name);

            StartCoroutine(authManager.LoadFormSave(PlayerPrefs.GetInt(" " + gameFile + "LoadFromSave")));
            StartCoroutine(authManager.SaveScene(PlayerPrefs.GetString(" " + character.gameFile + "LoadGame")));
            StartCoroutine(authManager.FacingLeft(PlayerPrefs.GetInt(" " + character.gameFile + "FacingLeft")));
            StartCoroutine(FadeIn());
        }

        protected virtual void OnDisable()
        {
            PlayerPrefs.SetInt(" " + character.gameFile + "FacingLeft", character.isFacingLeft ? 1 : 0);
            PlayerPrefs.SetString(" " + character.gameFile + "LoadGame", SceneManager.GetActiveScene().name);
        }

        public virtual void NextScene(SceneReference scene, int spawnReference)
        {
            PlayerPrefs.SetInt(" " + character.gameFile + "FacingLeft", character.isFacingLeft ? 1 : 0);
            PlayerPrefs.SetInt(" " + character.gameFile + "SpawnReference", spawnReference);
            PlayerPrefs.SetInt("Mission", stageToUnlock);
            PlayerPrefs.SetInt("Unlock", checkpointToUnlock);
            PlayerPrefs.SetInt(" " + character.gameFile + "CurrentHealth", player.GetComponent<PlayerHealth>().healthPoints);

            //Clear checkpoint Position
            PlayerPrefs.DeleteKey(" " + character.gameFile + "SaveSpawnReference");

            StartCoroutine(authManager.FacingLeft(PlayerPrefs.GetInt(" " + character.gameFile + "FacingLeft")));
            StartCoroutine(authManager.SpawnReference(PlayerPrefs.GetInt(" " + character.gameFile + "SpawnReference")));
            StartCoroutine(authManager.SaveSpawnReference(PlayerPrefs.GetInt(" " + character.gameFile + "SaveSpawnReference")));
            StartCoroutine(authManager.CurrentHealth(PlayerPrefs.GetInt(" " + character.gameFile + "CurrentHealth")));
            StartCoroutine(authManager.Mission(PlayerPrefs.GetInt("Mission")));
            StartCoroutine(authManager.Checkpoint(PlayerPrefs.GetInt("Unlock")));

            StartCoroutine(FadeOut(scene));
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

                dateText.alpha = currentColor.a;
                fadeScreen.color = currentColor;

                if (percentageComplete >= 1)
                {
                    fadeScreen.raycastTarget = false;
                    break;
                }
                    

                yield return new WaitForEndOfFrame();
            }
        }

        protected virtual IEnumerator FallFadeIn()
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / .5f;
            Color currentColor = fadeScreen.color;

            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / .5f;
                currentColor.a = Mathf.Lerp(1, 0, percentageComplete);

                dateText.alpha = currentColor.a;
                fadeScreen.color = currentColor;

                if (percentageComplete >= 1)
                {
                    fadeScreen.raycastTarget = false;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public virtual IEnumerator FallFadeOut()
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / .5f;
            Color currentColor = fadeScreen.color;

            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / .5f;
                currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
                fadeScreen.color = currentColor;
                dateText.text = "";
                if (percentageComplete >= 1)
                    break;

                yield return new WaitForEndOfFrame();
            }

            StartCoroutine(FallFadeIn());
        }

        protected virtual IEnumerator FadeOut(SceneReference scene)
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / fadeSpeed;
            Color currentColor = fadeScreen.color;
            dateText.text = "";

            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / fadeSpeed;
                currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
                dateText.alpha = currentColor.a;
                fadeScreen.color = currentColor;
                fadeScreen.raycastTarget = true;
                if (percentageComplete >= 1)
                {
                    soulImg.SetActive(true);
                    break;
                }
                    
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(scene);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(levelSize.center, levelSize.size);
        }
    }
}
