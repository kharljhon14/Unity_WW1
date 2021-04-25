using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace WorldWarOneTools
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Image fadeScreen;
        [SerializeField] private float fadeSpeed;
        [SerializeField] private BetterDialogue betterDialogue;
        [SerializeField] private SceneReference sceneReference;

        private bool isFading;

        private void Start()
        {
            StartCoroutine(FadeIn());
        }

        protected virtual void Update()
        {
            if(betterDialogue.index >= betterDialogue.sentences.Length - 1 && !isFading)
            {
                StartCoroutine(FadeOut(sceneReference));
            }
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
            isFading = true;

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

