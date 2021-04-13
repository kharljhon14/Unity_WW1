using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools 
{
    public class HorizontalMovement : Abilities
    {
        [SerializeField] protected float timeTillMaxSpeed; //Time to reach max speed
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float sprintMultiplier;
        [SerializeField] protected float crouchingSpeed;

        private float acceleration;
        private float currentSpeed;
        private float horizontalInput;
        private float runTime;

        protected override void Initialization()
        {
            base.Initialization(); //Initialize character class
        }

        protected virtual void Update()
        {
            MovementPressed();
            SprintingHeld();
        }

        public virtual bool MovementPressed()
        {
            //Check if movement button is pressed
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                horizontalInput = Input.GetAxisRaw("Horizontal");
                return true;
            }

            else
                return false;
        }

        protected virtual bool SprintingHeld()
        {
            //Check if sprint button is pressed
            if (Input.GetKey(KeyCode.LeftShift))
                return true;

            else
                return false;
        }

        protected virtual void FixedUpdate()
        {
            Movement();
        }

        protected virtual void Movement()
        {
            if (MovementPressed())
            {
                anim.SetBool("Moving", true);
                //Declare acceleration
                acceleration = maxSpeed / timeTillMaxSpeed; // Speed needs to ramp up to get to fullspeed. put 0 in timeTillMaxSpeed for no ramp up speed

                runTime += Time.fixedDeltaTime; //Calculate how many frames the player has been moving
                currentSpeed = horizontalInput * acceleration * runTime;

                CheckDirection();//All the logic for solving for maximum speed values
            }

            else
            {
                anim.SetBool("Moving", false);
                //Set all movement values to zero
                acceleration = 0;
                runTime = 0;
                currentSpeed = 0;
            }

            SpeedMultiplier();
            //Move Player
            anim.SetFloat("CurrentSpeed", currentSpeed);
            rb2d.velocity = new Vector2(currentSpeed, rb2d.velocity.y);
        }

        protected virtual void CheckDirection()
        {
            //Check the player direction
            if(currentSpeed > 0)
            {
                //if player is facing right
                if (character.isFacingLeft)
                {
                    character.isFacingLeft = false;
                    Flip();
                }

                //Limit the current 
                if (currentSpeed > maxSpeed)
                    currentSpeed = maxSpeed;
            }

            if(currentSpeed < 0)
            {
                if (!character.isFacingLeft)
                {
                    character.isFacingLeft = true;
                    Flip();
                }

                if (currentSpeed < -maxSpeed)
                    currentSpeed = -maxSpeed;
            }
           
        }

        protected virtual void SpeedMultiplier()
        {
            //if button is held multiply currentSpeed
            if (SprintingHeld())
                currentSpeed *= sprintMultiplier;

            if (character.isCrouching)
                currentSpeed *= crouchingSpeed;

            if(!character.isFacingLeft && CollisionCheck(Vector2.right, .05f, jump.collisionLayer) || character.isFacingLeft && CollisionCheck(Vector2.left, .05f, jump.collisionLayer))
                currentSpeed = .01f;
        }
    }
}

