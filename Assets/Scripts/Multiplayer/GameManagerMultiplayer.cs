using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

namespace WorldWarOneTools
{
    public class GameManagerMultiplayer : MonoBehaviourPunCallbacks
    {
        public static GameManagerMultiplayer Instance;

        public GameObject PlayerPrefabs;
        public GameObject GameCanvas;
        public GameObject SceneCamera;
        public TextMeshProUGUI PingText;

        [HideInInspector] public GameObject localPlayer;
        public TextMeshProUGUI respawnTimer;
        public GameObject respawnUI;
        private float timerAmount = 5f;

        private bool runSpawnTimer = false;
        private void Awake()
        {
            Instance = this;
            GameCanvas.SetActive(true);
        }

        private void Start()
        {
            SpawnPlayer();
        }

        private void Update()
        {
            PingText.text = "Ping: " + PhotonNetwork.GetPing();

            if (runSpawnTimer)
                StartRespawn();
        }

        public void EnableRespawn()
        {
            timerAmount = 5f;
            runSpawnTimer = true;
            respawnUI.SetActive(true);
        }

        private void StartRespawn()
        {
            timerAmount -= Time.deltaTime;
            respawnTimer.text = "Respawning in " + timerAmount.ToString("F0");

            if (timerAmount <= 0)
            {
                RespawnLocation();
                localPlayer.GetComponent<PhotonView>().RPC("Alive", RpcTarget.AllBuffered);
                localPlayer.GetComponent<MultiplayerHP>().CanMove();
                respawnUI.SetActive(false);
                runSpawnTimer = false;
            }
        }

        public void RespawnLocation()
        {
            float randomValue = Random.Range(40f, 35f);
            localPlayer.transform.localPosition = new Vector3(randomValue, 3f);
        }

        public void SpawnPlayer()
        {
            float randomValue = Random.Range(1f, 1f);
            PhotonNetwork.Instantiate(PlayerPrefabs.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
            GameCanvas.SetActive(false);
            SceneCamera.SetActive(true);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(1);
        }
    }
}
