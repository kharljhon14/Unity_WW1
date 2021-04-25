using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldWarOneTools
{
    public class SaveGame : Manager
    {
        [SerializeField] protected int reference;
        [SerializeField] private int levelCheckpoint;
        [SerializeField] private GameObject button;
        private AuthManager authManager;

        protected override void Initialization()
        {
            base.Initialization();
            authManager = FindObjectOfType<AuthManager>();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Save();
                button.SetActive(true);
                if(SimpleInput.GetButton("Fire2"))
                    Save();
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Save();
                if (SimpleInput.GetButton("Fire2"))
                {
                    Save();
                    GetComponent<CheckpointCutScene>().StartCutScene();
                }    
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            button.SetActive(false);
        }

        protected virtual void Save()
        {
            PlayerPrefs.SetString(" " + character.gameFile + "LoadGame", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetInt(" " + character.gameFile + "SaveSpawnReference", reference);
            PlayerPrefs.SetInt(" " + character.gameFile + "FacingLeft", character.isFacingLeft ? 1 : 0);
            PlayerPrefs.SetInt("Unlock", levelCheckpoint);
            player.GetComponent<Health>().healthPoints = player.GetComponent<Health>().maxHealthPoints;

            StartCoroutine(authManager.SaveScene(PlayerPrefs.GetString(" " + character.gameFile + "LoadGame")));
            StartCoroutine(authManager.SaveSpawnReference((PlayerPrefs.GetInt(" " + character.gameFile + "SaveSpawnReference"))));
            StartCoroutine(authManager.FacingLeft(PlayerPrefs.GetInt(" " + character.gameFile + "FacingLeft")));
            StartCoroutine(authManager.CurrentHealth(PlayerPrefs.GetInt(" " + character.gameFile + "CurrentHealth")));
            StartCoroutine(authManager.Checkpoint(PlayerPrefs.GetInt("Unlock")));
        }

    }
}
