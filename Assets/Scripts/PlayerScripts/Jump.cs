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
        [SerializeField] protected float maxJumpSpeed;
        [SerializeField] protected float maxFallSpeed;
        [SerializeField] protected float acceptedFallSpeed;
        [SerializeField] protected LayerMask collisionLayer;

        private bool isJumping;
        private float jumpCountDown;
        private int numberOfJumpsLeft;

        protected override void Initialization()
        {
            base.Initialization();
            numberOfJumpsLeft = maxJumps;
            jumpCountDown = buttonHoldTime;
        }

        protected virtual void Update()
        {
            JumpPressed();
            JumpHeld();
        }

        protected virtual bool JumpPressed()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //To Check if the player is falling and disable jumping
                if (!character.isGrounded && numberOfJumpsLeft == maxJumps)
                {
                    isJumping = false;
                    return false;
                }

                if(limitAirJumps && Falling(acceptedFallSpeed))//Check to see if want to limit air jump
                {
                    isJumping = false;
                    return false;
                }

                numberOfJumpsLeft--;
                if(numberOfJumpsLeft >= 0)
                {
                    jumpCountDown = buttonHoldTime;
                    isJumping = true;
                }
                    
                return true;
            }
               
            else
                return false;
        }

        //Check if jump button is held
        protected virtual bool JumpHeld()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                return true;
            }
            else
                return false;
        }

        protected virtual void FixedUpdate()
        {
            IsJumping();
            GroundCheck();
        }

        protected virtual void IsJumping()
        {
            //Add Y velocity force in the rigidbody
            if (isJumping)
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

        protected virtual void AdditionalAir()
        {
            if (JumpHeld())
            {
                jumpCountDown -= Time.deltaTime;
                if (jumpCountDown <= 0)
                {
                    jumpCountDown = 0;
                    isJumping = false;
                }
                else
                    rb2d.AddForce(Vector2.up * holdForce);
            }
            else
                isJumping = false;
        }

        protected virtual void GroundCheck()
        {
            //Checking if the player is grounded and reseting the numberOfJumpsLeft
            if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !isJumping)
            {
                anim.SetBool("Grounded", true);
                character.isGrounded = true;
                numberOfJumpsLeft = maxJumps;
            }

            else
            {
                anim.SetBool("Grounded", false);
                character.isGrounded = false;

                if(Falling(0) && rb2d.velocity.y < maxFallSpeed) //Limit the FallSpeed;
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, maxFallSpeed);
                }
            }
            anim.SetFloat("VerticalSpeed", rb2d.velocity.y);
        }
    }

}
