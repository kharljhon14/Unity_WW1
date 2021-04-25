using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Ladder : PlatformManager
    {
        [HideInInspector] public Vector3 topOfLadder;
        [HideInInspector] public Vector3 bottomOfLadder;
        protected HorizontalMovement movement;

        protected override void Initialization()
        {
            base.Initialization();
            movement = player.GetComponent<HorizontalMovement>();
            topOfLadder = new Vector3(transform.position.x, platformCollider.bounds.max.y);
            bottomOfLadder = new Vector3(transform.position.x, platformCollider.bounds.min.y);
            platformCollider.isTrigger = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject == player)
            {
                character.isOnLadder = false;
                movement.currentLadder = null;      
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.gameObject == player)
            {
                if (character.isJumping)
                {
                    character.isOnLadder = false;
                    movement.currentLadder = null;
                    return;
                }

                else
                {
                    character.isOnLadder = true;
                    movement.currentLadder = gameObject;
                }
            }
        }
    }

}
