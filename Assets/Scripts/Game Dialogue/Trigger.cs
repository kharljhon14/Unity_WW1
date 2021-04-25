using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject soul;
    [SerializeField] private GameObject dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(soul != null)
                soul.SetActive(true);

            dialogue.SetActive(true);
            Destroy(gameObject, 1);
        }
    }
}
