using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools 
{
    public class HorizontalMovement : Abilities
    {
        [SerializeField] protected float timeTillMaxSpeed;
        [SerializeField] protected float maxSpeed;

        private float acceleration;
        private float currentSpeed;
        private float horizontalInput;
        private float runTime;

        protected override void Initialization()
        {
            base.Initialization();
        }

        protected virtual void Update()
        {
            MovementPressed();
        }

        protected virtual bool MovementPressed()
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                return true;
            }
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
                //Declare acceleration
                acceleration = maxSpeed / timeTillMaxSpeed; // Speed needs to ramp up to get to fullspeed. put 0 in timeTillMaxSpeed for no ramp up speed

                runTime += Time.fixedDeltaTime; //Calculate how many frames the player has been moving
                currentSpeed = horizontalInput * acceleration * runTime;

                CheckDirection();//All the logic for solving for maximum speed values
            }
            else
            {
                //Set all movement values to zero
                acceleration = 0;
                runTime = 0;
                currentSpeed = 0;
            }
            //Move Player
            rb2d.velocity = new Vector2(currentSpeed, rb2d.velocity.y);
        }

        protected virtual void CheckDirection()
        {
            //Limit the current 
            if (currentSpeed > maxSpeed)
                currentSpeed = maxSpeed;

            if (currentSpeed < -maxSpeed)
                currentSpeed = -maxSpeed;
        }
    }
}

