using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    [RequireComponent (typeof(CapsuleCollider2D))]
    public class Crouch : Abilities
    {
        [Range(0, 1)] [SerializeField] protected float colliderMultiplier;
        [SerializeField] protected LayerMask layers;

        private CapsuleCollider2D playerCollider;
        private Vector2 originalCollider;
        private Vector2 crouchingCollider;
        private Vector2 originalOffset;
        private Vector2 crouchingOffset;

        protected override void Initialization()
        {
            base.Initialization();
            playerCollider = GetComponent<CapsuleCollider2D>();
            originalCollider = playerCollider.size;
            crouchingCollider = new Vector2(playerCollider.size.x, (2.67096f));
            originalOffset = playerCollider.offset;
            //crouchingOffset = new Vector2(playerCollider.offset.x, (playerCollider.offset.y * colliderMultiplier));
            crouchingOffset = new Vector2(originalOffset.x, -0.5604947f);
        }

        protected virtual void FixedUpdate()
        {
            Crouching();
        }

        protected virtual void Crouching()
        {
            if(inputManager.CrouchHeld() && character.isGrounded)
            {
                character.isCrouching = true;
                //Need Crouching Animation
                //anim.SetBool("Crouching", true);
                playerCollider.size = crouchingCollider;
                playerCollider.offset = crouchingOffset;
            }
            else
            {
                if (character.isCrouching)
                {
                    if(CollisionCheck(Vector2.up, playerCollider.size.y * .25f, layers))
                    {
                        return;
                    }
                        StartCoroutine(CrouchDisable());
                }      
            }
        }

        protected virtual IEnumerator CrouchDisable()
        {
            playerCollider.offset = originalOffset;
            yield return new WaitForSeconds(.01f);
            playerCollider.size = originalCollider;
            yield return new WaitForSeconds(.15f);
            character.isCrouching = false;
            //Need Crouching Animation
            //anim.SetBool("Crouching", false);
        }
    }
}

