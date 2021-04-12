using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Character : MonoBehaviour
    {
        protected Collider2D col;
        protected Rigidbody2D rb2d;

        private void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            //Get player Collider and RigidBody
            col = GetComponent<Collider2D>();
            rb2d = GetComponent<Rigidbody2D>();
        }
    }
}

