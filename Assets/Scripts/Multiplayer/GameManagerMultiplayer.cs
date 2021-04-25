using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class GameManagerMultiplayer : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefabs;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public TextMeshProUGUI PingText;
    public GameObject DisconnectUI;
    private bool Off = false;

    public GameObject PlayerFeed;
    public GameObject FeedGrid;
    private void Awake()
    {
        GameCanvas.SetActive(true);
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        CheckInput();
        PingText.text = "Ping: " + PhotonNetwork.GetPing();
    }

    public void CheckInput()
    {
        if(Off && Input.GetKeyDown(KeyCode.Escape))
        {
            DisconnectUI.SetActive(false);
            Off = false;
        }
        else if(!Off && Input.GetKeyDown(KeyCode.Escape))
        {
            DisconnectUI.SetActive(true);
            Off = true;
        }
    }
    public void SpawnPlayer()
    {
        float randomValue = Random.Range(1f, 1f);
        PhotonNetwork.Instantiate(PlayerPrefabs.name, new Vector2(this.transform.position.x *randomValue, this.transform.position.y), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        GameObject obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(FeedGrid.transform, false);
        obj.GetComponent<Text>().text = player.NickName + "Joined the game";
        obj.GetComponent<Text>().color = Color.green;
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        GameObject obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(FeedGrid.transform, false);
        obj.GetComponent<Text>().text = player.NickName + "Left the game";
        obj.GetComponent<Text>().color = Color.red;
    }
}
