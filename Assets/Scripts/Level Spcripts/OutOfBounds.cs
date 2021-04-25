using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class OutOfBounds : Manager
    {
        [SerializeField] protected bool specificLocation;
        [SerializeField] protected Vector3 location;

        protected override void Initialization()
        {
            base.Initialization();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (specificLocation)
                    player.transform.position = location;

                else
                    player.transform.position = player.GetComponent<HorizontalMovement>().bestDeltaPosition;

                StartCoroutine(levelManager.FallFadeOut());
            }
        }

        private void OnDrawGizmos()
        {
            if (specificLocation)
            {
                Gizmos.DrawSphere(location, 1);
            }
        }
    }
}
