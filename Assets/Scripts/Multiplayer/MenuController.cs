using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

namespace WorldWarOneTools
{
    public class MenuController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject ConnectPanel;

        [SerializeField] private TMP_InputField CreateGameInput;
        [SerializeField] private TMP_InputField JoinGameInput;

        private AuthManager authManager;

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
            authManager = FindObjectOfType<AuthManager>();
        }

        private void Start()
        {
            PhotonNetwork.NickName = authManager.user.DisplayName;
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene(1);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("Connected");
        }

        public void CreateGame()
        {
            PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { MaxPlayers = 5 }, null);
        }
        public void JoinGame()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 5;
            PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Arena");
        }
    }

}