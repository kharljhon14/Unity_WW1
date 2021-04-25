using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools 
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlatformManager : Manager
    {
        protected BoxCollider2D platformCollider;
        protected Rigidbody2D platformRb2d;
        protected LayerMask playerLayer;

        protected override void Initialization()
        {
            base.Initialization();
            platformCollider = GetComponent<BoxCollider2D>();
            platformRb2d = GetComponent<Rigidbody2D>();
            playerLayer = LayerMask.GetMask("Player");
        }

        protected virtual void FixedUpdate()
        {
            CollisionCheck();
        }
        
        protected virtual bool CollisionCheck()
        {
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y), new Vector2(platformCollider.bounds.size.x -.1f, .05f), 0f, Vector2.up, .05f, playerLayer);

            if (hit)
                return true;

            return false;
        }
    }

}
