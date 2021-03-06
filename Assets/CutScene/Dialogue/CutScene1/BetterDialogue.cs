using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace WorldWarOneTools
{
    public class BetterDialogue : MonoBehaviour
    {
        public TextMeshProUGUI textDisplay;
        public string[] sentences;
        [HideInInspector] public int index;
        public float typingSpeed;

        public GameObject objects;

        public GameObject continueButton;


        private void Start()
        {
            StartCoroutine(Type());
        }

        private void Update()
        {
            if (textDisplay.text == sentences[index])
            {
                continueButton.SetActive(true);
            }
        }

        IEnumerator Type()
        {
            foreach (char letter in sentences[index].ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        public void NextSentence()
        {
            continueButton.SetActive(false);

            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine(Type());
            }

            else
            {
                if (objects != null)
                    Destroy(objects);

                textDisplay.text = "";
                continueButton.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}