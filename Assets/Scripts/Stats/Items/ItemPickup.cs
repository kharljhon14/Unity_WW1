using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class ItemPickup : MonoBehaviour
    {
        public ItemType item;

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                item.UseItem(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}

