using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace WorldWarOneTools
{
    public class CharacterSelect : MonoBehaviour
    {
        private AuthManager authManager;

        [SerializeField] private Image fadeScreen;
        [SerializeField] private float fadeSpeed;
        [SerializeField] private GameObject confirmationUI;

        private void Start()
        {
            StartCoroutine(FadeIn());
            authManager = FindObjectOfType<AuthManager>();
        }

        public void AreYouSure()
        {
            confirmationUI.SetActive(true);
        }

        public void Yes()
        {
            confirmationUI.SetActive(false);
            StartCoroutine(FadeOutContinue("Tutorial"));
            StartCoroutine(authManager.Character(PlayerPrefs.GetInt("char")));
        }

        public void No()
        {
            confirmationUI.SetActive(false);
        }

        public void Grunt()
        {
            PlayerPrefs.SetInt("char", 0);
            confirmationUI.SetActive(true);  
        }

        public void Commando()
        {
            PlayerPrefs.SetInt("char", 1);
            confirmationUI.SetActive(true);
        }

        public void Rambo()
        {
            PlayerPrefs.SetInt("char", 2);
            confirmationUI.SetActive(true);
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
