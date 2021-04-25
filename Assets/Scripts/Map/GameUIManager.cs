using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldWarOneTools
{
    public class GameUIManager : Manager
    {
        public SceneReference mainMenuScene;
        public GameObject deathScreen;
        public GameObject gamePausedScreen;
        public GameObject areYouSureScreen;

        protected float originalTimeScale;

        protected override void Initialization()
        {
            base.Initialization();
            ManageUI();
        }

        public virtual void GamePaused()
        {
            if (!gameManager.gamePaused)
            {
                gamePausedScreen.SetActive(true);
                gameManager.gamePaused = true;
                originalTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                gamePausedScreen.SetActive(false);
                areYouSureScreen.SetActive(false);
                gameManager.gamePaused = false;
                Time.timeScale = originalTimeScale;
            }
        }

        public virtual void ReturnToGame()
        {
            gamePausedScreen.SetActive(false);
            gameManager.gamePaused = false;
            Time.timeScale = originalTimeScale;
        }

        public virtual void QuitGame()
        {
            areYouSureScreen.SetActive(true);
        }

        public virtual void ReturnToMainMenu()
        {
            areYouSureScreen.SetActive(false);
            gamePausedScreen.SetActive(true);
        }

        public virtual void ManageUI()
        {
            areYouSureScreen.SetActive(false);
            gamePausedScreen.SetActive(false);
            deathScreen.SetActive(false);
        }

        public virtual void SureToQuit()
        {
            PlayerPrefs.SetString(" " + character.gameFile + "LoadGame", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(mainMenuScene);
            Time.timeScale = originalTimeScale;
            gameManager.gamePaused = false;
        }
    }
}

