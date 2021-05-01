using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace WorldWarOneTools
{
    public class MissionManager : MonoBehaviour
    {
        [SerializeField] private SceneReference mainMenu;

        [SerializeField] private Image fadeScreen;
        [SerializeField] private float fadeSpeed;

        [SerializeField] private SceneReference stage1;
        [SerializeField] private SceneReference stage2;
        [SerializeField] private SceneReference stage3;
        [SerializeField] private SceneReference stage4;
        [SerializeField] private SceneReference stage5;
        [SerializeField] private SceneReference stage6;
        [SerializeField] private SceneReference multiplayer;

        [SerializeField] private GameObject levelDetailPanel;
        [SerializeField] private TextMeshProUGUI stage;
        [SerializeField] private TextMeshProUGUI details;

        private int stageCount = 0;

        private void Start()
        {
            StartCoroutine(FadeIn());
            Debug.Log(PlayerPrefs.GetInt("Unlock"));
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(mainMenu);
        }

        public void ShowLevelDetailsOne()
        {
            stageCount = 1;
            levelDetailPanel.SetActive(true);
            stage.text = "Stage 1";
            details.text = "Tutorial";
        }

        public void ShowLevelDetailsTwo()
        {
            stageCount = 2;
            levelDetailPanel.SetActive(true);
            stage.text = "Stage 2";
            details.text = "Gavrilo Princip, a Serbian nationalist, was order to assassinated the Austrian heir to the throne, Archduke Franz Ferdinand and his wife Sophie in the Bosnian capital Sarajevo";
        }

        public void ShowLevelDetailsThree()
        {
            stageCount = 3;
            levelDetailPanel.SetActive(true);
            stage.text = "Stage 3";
            details.text = "Gavrilo Princip, a Serbian nationalist, was order to assassinated the Austrian heir to the throne, Archduke Franz Ferdinand and his wife Sophie in the Bosnian capital Sarajevo";
        }

        public void ShowLevelDetailsFour()
        {
            stageCount = 4;
            levelDetailPanel.SetActive(true);
            stage.text = "Stage 4";
            details.text = "Gavrilo Princip, a Serbian nationalist, was order to assassinated the Austrian heir to the throne, Archduke Franz Ferdinand and his wife Sophie in the Bosnian capital Sarajevo";
        }

        public void ShowLevelDetailsFive()
        {
            stageCount = 5;
            levelDetailPanel.SetActive(true);
            stage.text = "Stage 4";
            details.text = "Gavrilo Princip, a Serbian nationalist, was order to assassinated the Austrian heir to the throne, Archduke Franz Ferdinand and his wife Sophie in the Bosnian capital Sarajevo";
        }

        public void ShowLevelDetailsSix()
        {
            stageCount = 6;
            levelDetailPanel.SetActive(true);
            stage.text = "Stage 5";
            details.text = "Gavrilo Princip, a Serbian nationalist, was order to assassinated the Austrian heir to the throne, Archduke Franz Ferdinand and his wife Sophie in the Bosnian capital Sarajevo";
        }

        public void HideLevelDetails()
        {
            levelDetailPanel.SetActive(false);
        }

        public void StageOneStartGame()
        {
            bool loadFromSave = false;
            PlayerPrefs.SetInt(" " + 1 + "LoadFromSave", loadFromSave ? 1 : 0);
            PlayerPrefs.SetInt(" " + 1 + "SaveSpawnReference", 0);
            switch (stageCount)
            {
                case 1:
                    StartCoroutine(FadeOut(stage1));
                    break;

                case 2:
                    StartCoroutine(FadeOut(stage2));
                    break;

                case 3:
                    StartCoroutine(FadeOut(stage3));
                    break;

                case 4:
                    StartCoroutine(FadeOut(stage4));
                    break;

                case 5:
                    StartCoroutine(FadeOut(stage5));
                    break;

                case 6:
                    StartCoroutine(FadeOut(stage6));
                    break;
            }
        }

        public void StageOneContinueGame()
        {
            bool loadFromSave = true;
            PlayerPrefs.SetInt(" " + 1 + "LoadFromSave", loadFromSave ? 1 : 0);
            PlayerPrefs.SetInt(" " + 1 + "SaveSpawnReference", 1);

            switch (stageCount)
            {
                case 1:
                    if(PlayerPrefs.GetInt("Unlock") == 1)
                        StartCoroutine(FadeOut(stage1));
                    break;

                case 2:
                    if (PlayerPrefs.GetInt("Unlock") >= 2)
                        StartCoroutine(FadeOut(stage2));
                    break;

                case 3:
                    if (PlayerPrefs.GetInt("Unlock") >= 3)
                        StartCoroutine(FadeOut(stage3));
                    break;

                case 4:
                    if (PlayerPrefs.GetInt("Unlock") >= 4)
                        StartCoroutine(FadeOut(stage4));
                    break;

                case 5:
                    if (PlayerPrefs.GetInt("Unlock") >= 5)
                        StartCoroutine(FadeOut(stage5));
                    break;

                case 6:
                    if (PlayerPrefs.GetInt("Unlock") >= 6)
                        StartCoroutine(FadeOut(stage6));
                    break;
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

        public void Multiplayer()
        {
            StartCoroutine(FadeOut(multiplayer));
        }
    }
}

