using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Character : MonoBehaviour
    {
        [HideInInspector] public bool isFacingLeft;

        protected Collider2D col;
        protected Rigidbody2D rb2d;

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
            facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        protected virtual void Flip()
        {
            if (isFacingLeft)
            {
                transform.localScale = facingLeft;
            }
            else
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
    }
}

