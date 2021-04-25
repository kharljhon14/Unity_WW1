using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WorldWarOneTools 
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        //Screen object variables
        public GameObject loginUI;
        public GameObject registerUI;
        public GameObject verifyEmailUI;
        public GameObject checkingForAccountUI;
        public TextMeshProUGUI verifyEmailText;

        public SceneReference newGameScene;

        public List<AbiltyItem> abilitiesToClear = new List<AbiltyItem>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }
        }

        private void ClearUI()
        {
            AuthManager.instance.ClearOutputs();
            loginUI.SetActive(false);
            registerUI.SetActive(false);
            verifyEmailUI.SetActive(false);
            AuthManager.instance.ClearOutputs();
            checkingForAccountUI.SetActive(false);
        }

        //Functions to change the login screen UI
        public void LoginScreen() //Back button
        {
            ClearUI();
            loginUI.SetActive(true);
        }
        public void RegisterScreen() // Regester button
        {
            ClearUI();
            registerUI.SetActive(true);
        }

        public void AwaitVerification(bool _emailSent, string _email, string _output)
        {
            ClearUI();
            verifyEmailUI.SetActive(true);
            if (_emailSent)
            {
                verifyEmailText.text = $"Sent Email\nPlease Verify {_email})";
            }
            else
            {
                verifyEmailText.text = $"Email Not Sent: {_output}\nPlease Verify {_email}";
            }
        } 
    }
}
