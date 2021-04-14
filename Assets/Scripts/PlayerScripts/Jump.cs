using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools 
{
    public class Jump : Abilities
    {
        [SerializeField] protected bool limitAirJumps;

        [SerializeField] protected int maxJumps;
        [SerializeField] protected float jumpForce;
        [SerializeField] protected float holdForce;
        [SerializeField] protected float buttonHoldTime;

        [SerializeField] protected float distanceToCollider;
        [SerializeField] protected float horizontalWallJumpForce;
        [SerializeField] protected float verticalWallJumpForce;
        [SerializeField] protected float maxJumpSpeed;
        [SerializeField] protected float maxFallSpeed;
        [SerializeField] protected float acceptedFallSpeed;

        [SerializeField] protected float glideTime;
        [Range(-2, 2)] [SerializeField] protected float gravity;
        [SerializeField] protected float wallJumpTime;
        public LayerMask collisionLayer;

        private bool isWallJumping;
        private float jumpCountDown;
        private int numberOfJumpsLeft;
        private float fallCountDown;

        protected override void Initialization()
        {
            base.Initialization();
            numberOfJumpsLeft = maxJumps;
            jumpCountDown = buttonHoldTime;
            fallCountDown = glideTime;
        }

        protected virtual void Update()
        {
            CheckForJump();
        }

        protected virtual bool CheckForJump()
        {
            if (inputManager.JumpPressed())
            {
                //To Check if the player is falling and disable jumping
                if (!character.isGrounded && numberOfJumpsLeft == maxJumps)
                {
                    character.isJumping = false;
                    return false;
                }

                if(limitAirJumps && character.Falling(acceptedFallSpeed))//Check to see if want to limit air jump
                {
                    character.isJumping = false;
                    return false;
                }

                if (character.isWallSliding)
                {
                    isWallJumping = true;
                    return false;
                }

                numberOfJumpsLeft--;
                if(numberOfJumpsLeft >= 0)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    jumpCountDown = buttonHoldTime;
                    character.isJumping = true;
                    fallCountDown = glideTime;
                }
                    
                return true;
            }
               
            else
                return false;
        }

      
        protected virtual void FixedUpdate()
        {
            IsJumping();
            //Gliding();
            GroundCheck();
            //WallSliding();
            //WallJump();
        }

        protected virtual void IsJumping()
        {
            //Add Y velocity force in the rigidbody
<<<<<<< Updated upstream
            if (isJumping && !character.isCrouching)
=======
            if (character.isJumping && !character.isCrouching)
>>>>>>> Stashed changes
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                rb2d.AddForce(Vector2.up * jumpForce);
                AdditionalAir();
            }

            if(rb2d.velocity.y > maxJumpSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, maxJumpSpeed);
            }
        }

        protected virtual void Gliding()
        {
<<<<<<< Updated upstream
            if(Falling(0) && inputManager.JumpHeld())
=======
            if(character.Falling(0) && inputManager.JumpHeld())
>>>>>>> Stashed changes
            {
                fallCountDown -= Time.deltaTime;
                if(fallCountDown > 0 && rb2d.velocity.y > acceptedFallSpeed)
                {
                    FallSpeed(gravity);
                }
            }
        }

        protected virtual void AdditionalAir()
        {
            if (inputManager.JumpHeld())
            {
                jumpCountDown -= Time.deltaTime;
                if (jumpCountDown <= 0)
                {
                    jumpCountDown = 0;
                    character.isJumping = false;
                }
                else
                    rb2d.AddForce(Vector2.up * holdForce);
            }
            else
                character.isJumping = false;
        }

        protected virtual void GroundCheck()
        {
            //Checking if the player is grounded and reseting the numberOfJumpsLeft
            if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !isJumping)
            {
                anim.SetBool("Grounded", true);
                character.isGrounded = true;
                numberOfJumpsLeft = maxJumps;
                fallCountDown = glideTime;
            }

            else
            {
                anim.SetBool("Grounded", false);
                character.isGrounded = false;

                if(character.Falling(0) && rb2d.velocity.y < maxFallSpeed) //Limit the FallSpeed;
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, maxFallSpeed);
                }
            }
            anim.SetFloat("VerticalSpeed", rb2d.velocity.y);
        }

        protected virtual bool WallCheck()
        {
            //Check if there is wall infront of the player regardless of what direction the player is facing and checking if the player is not grounded and if the player is moving
            if((!character.isFacingLeft && CollisionCheck(Vector2.right, distanceToCollider, collisionLayer) || character.isFacingLeft && CollisionCheck(Vector2.left, distanceToCollider, collisionLayer)) && inputManager.MovementPressed() && !character.isGrounded)
            {
                return true;
            }
            return false;
        }

        protected virtual bool WallSliding()
        {
            if (WallCheck())
            {
                FallSpeed(gravity);
                character.isWallSliding = true;
                return true;
            }

            else
            {
                character.isWallSliding = false;
                return false;
            }
        }

        protected virtual void WallJump()
        {
            if (isWallJumping)
            {
                rb2d.AddForce(Vector2.up * verticalWallJumpForce);
                if (!character.isFacingLeft)
                {
                    rb2d.AddForce(Vector2.left * horizontalWallJumpForce);
                }

                if (character.isFacingLeft)
                {
                    rb2d.AddForce(Vector2.right * horizontalWallJumpForce);
                }

                StartCoroutine(WallJumped());
            }
        }

        protected virtual IEnumerator WallJumped()
        {
            horizontalMovement.enabled = false;
            yield return new WaitForSeconds(wallJumpTime);
            horizontalMovement.enabled = true;
            isWallJumping = false;
        }
    }

}
