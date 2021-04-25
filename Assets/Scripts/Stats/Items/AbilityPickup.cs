using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class AbilityPickup : MonoBehaviour
    {
        public ItemType item;
        protected bool found;

        private void OnEnable()
        {
            found = PlayerPrefs.GetInt(" " + PlayerPrefs.GetInt("GameFile") + item) == 1 ? true : false;

            if (found)
                Destroy(gameObject);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                item.UseItem(collision.gameObject);
                found = true;
                PlayerPrefs.SetInt(" " + PlayerPrefs.GetInt("GameFile") + item, found ? 1 : 0);
                Destroy(gameObject);
            }
        }
    }
}
