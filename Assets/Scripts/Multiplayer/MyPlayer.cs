using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace WorldWarOneTools
{
    public class MyPlayer : MonoBehaviourPunCallbacks
    {
        public PhotonView photonView;
        public GameObject playerCamera;
        private SpriteRenderer sr;
        private Animator anim;

        public TextMeshProUGUI username;
        public float moveSpeed;

        public GameObject bullet;
        public Transform firePos;

        private void Awake()
        {

            if (photonView.IsMine)
                username.text = PhotonNetwork.NickName;

            else
                username.text = photonView.Owner.NickName;

        }

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }


        private void Update()
        {
            if (photonView.IsMine)
            {
                CheckInputs();
            }
        }

        private void Shoot()
        {
            if(sr.flipX == false)
            {
                GameObject newBullet = PhotonNetwork.Instantiate(bullet.name, new Vector2(firePos.position.x, firePos.position.y), Quaternion.identity, 0);
            }
              

            if (sr.flipX == true)
            {
                GameObject newBullet = PhotonNetwork.Instantiate(bullet.name, new Vector2(firePos.position.x, firePos.position.y), Quaternion.identity, 0);
                newBullet.GetComponent<PhotonView>().RPC("ChangeDirLeft", RpcTarget.AllBuffered);
            }
        }

        private void CheckInputs()
        {
            var move = new Vector3(SimpleInput.GetAxisRaw("Horizontal"), 0);
            transform.position += move * moveSpeed * Time.deltaTime;

            if (SimpleInput.GetButton("Fire3"))
                Shoot();

            //if (SimpleInput.GetButtonDown("Fire3"))
            //    Shoot();

            if (move.x > 0)
            {
                anim.SetBool("Moving", true);
                photonView.RPC("FlipFalse", RpcTarget.AllBuffered);
            }
               
            else if (move.x < 0)
            {
                anim.SetBool("Moving", true);
                photonView.RPC("FlipTrue", RpcTarget.AllBuffered);
            }

            if(move.x == 0)
                anim.SetBool("Moving", false);

        }

        [PunRPC]
        private void FlipTrue()
        {
            sr.flipX = true;
        }

        [PunRPC]
        private void FlipFalse()
        {
            sr.flipX = false;
        }
    }
}
