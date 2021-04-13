using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools 
{
    public class Dash : Abilities
    {
        [SerializeField] protected float dashForce;
        [SerializeField] protected float dashCooldownTime;
        [SerializeField] protected float dashAmountTime;
        [SerializeField] protected LayerMask dashingLayer;

        private bool canDash;
        private float dashCountDown;
        private CapsuleCollider2D capsuleCollider;

        protected override void Initialization()
        {
            base.Initialization();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        protected virtual void Update()
        {
            Dashing();
        }

        protected virtual void Dashing()
        {
            if (inputManager.DashPressed() && canDash && isGrounded)
            {
                dashCountDown = dashCooldownTime;
                capsuleCollider.direction = CapsuleDirection2D.Horizontal;
                capsuleCollider.size = new Vector2(capsuleCollider.size.y, capsuleCollider.size.x);
                character.isDashing = true;
                StartCoroutine(FinishedDashing());
            }    
        }

        protected virtual void FixedUpdate()
        {
            DashMode();
            ResetDashCounter();
        }

        protected virtual void DashMode()
        {
            if (character.isDashing)
            {
                FallSpeed(0);
                horizontalMovement.enabled = false;
                if (!character.isFacingLeft)
                {
                    DashCollision(Vector2.right, .5f, dashingLayer);
                    rb2d.AddForce(Vector2.right * dashForce);
                }

                else
                {
                    DashCollision(Vector2.left, .5f, dashingLayer);
                    rb2d.AddForce(Vector2.left * dashForce);
                }            
            }
        }

        protected virtual void DashCollision(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                //Check the hit collider by the collision mask 
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    hits[i].collider.enabled = false;
                    StartCoroutine(TurnColliderBackOn(hits[i].collider.gameObject));
                }
            }
        }

        protected virtual void ResetDashCounter()
        {
            if(dashCountDown > 0)
            {
                canDash = false;
                dashCountDown -= Time.deltaTime;
            }
            else
            {
                canDash = true;
            }
        }

        protected virtual IEnumerator FinishedDashing()
        {
            yield return new WaitForSeconds(dashAmountTime);
            capsuleCollider.direction = CapsuleDirection2D.Vertical;
            capsuleCollider.size = new Vector2(capsuleCollider.size.y, capsuleCollider.size.x);
            character.isDashing = false;
            FallSpeed(1);
            horizontalMovement.enabled = true;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        protected virtual IEnumerator TurnColliderBackOn(GameObject obj)
        {
            yield return new WaitForSeconds(dashAmountTime);
            obj.GetComponent<Collider2D>().enabled = true;
        }
    }
}

