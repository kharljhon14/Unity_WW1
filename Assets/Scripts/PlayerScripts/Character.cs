using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Character : MonoBehaviour
    {
        [HideInInspector] public bool isFacingLeft;
        [HideInInspector] public bool isGrounded;

        protected Collider2D col;
        protected Rigidbody2D rb2d;
        protected Animator anim;

        private Vector2 facingLeft;

        private void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            //Get player Collider and RigidBody
            col = GetComponent<Collider2D>();
            rb2d = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        protected virtual void Flip()
        {
            if (isFacingLeft)
                transform.localScale = facingLeft;
            else
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        //Check if player is grounded
        protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                //Check the hit collider by the collision mask 
                if((1 << hits[i].collider.gameObject.layer & collision) != 0)
                    return true;
            }
            return false;
        }

        protected virtual bool Falling(float velocity)
        {
            if (!isGrounded && rb2d.velocity.y < velocity)
                return true;
            else
                return false;
        }
    }
}

