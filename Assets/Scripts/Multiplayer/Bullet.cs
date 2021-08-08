using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    public bool moveDir;
    public float moveSpeed;
    public float destroyTime;

    public float bulletDmg;

    private void Start()
    {
        StartCoroutine(DestroyTime());
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ChangeDirLeft()
    {
        moveDir = true;
    }

    [PunRPC]
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (!moveDir)
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        else
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.IsMine || target.IsRoomView))
        {
            if (target.CompareTag("Player"))
            {
                target.RPC("ReduceHealth", RpcTarget.AllBuffered, bulletDmg);
                this.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
            }
        }

        if (collision.CompareTag("Platform"))
        {
            Destroy(gameObject);
        }
    }
}
