using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace WorldWarOneTools
{

    public class MultiplayerHP : MonoBehaviourPunCallbacks
    {
        public float healthAmount;
        public Image hp;


        public MyPlayer player;
        public GameObject playerUI;
        public Rigidbody2D rb2d;
        public Collider2D col;
        public SpriteRenderer sr;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                GameManagerMultiplayer.Instance.localPlayer = this.gameObject;
            }
        }

        [PunRPC]
        public void ReduceHealth(float amount)
        {
            AudioManager.instance.PlaySFX(1);
            ModifyHealth(amount);
        }

        private void CheckHealth()
        {
            hp.fillAmount = healthAmount / 100f;

            if (photonView.IsMine && healthAmount <= 0)
            {
                GameManagerMultiplayer.Instance.EnableRespawn();
                player.cantMove = true;
                this.GetComponent<PhotonView>().RPC("Dead", RpcTarget.AllBuffered);
            }
        }

        public void CanMove()
        {
            player.cantMove = false;
        }

        [PunRPC]
        private void Dead()
        {
            rb2d.gravityScale = 0;
            col.enabled = false;
            sr.enabled = false;
            playerUI.SetActive(false);
        }

        [PunRPC]
        private void Alive()
        {
            rb2d.gravityScale = 1;
            col.enabled = true;
            sr.enabled = true;
            playerUI.SetActive(true);
            hp.fillAmount = 1;
            healthAmount = 100;
        }

        private void ModifyHealth(float amount)
        {
            if (photonView.IsMine)
            {
                healthAmount -= amount;
                hp.fillAmount -= amount;
            }

            else
            {
                healthAmount -= amount;
                hp.fillAmount -= amount;
            }

            CheckHealth();
        }
    }
}
