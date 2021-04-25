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
        [SerializeField] protected float ladderSpeed;

        public List<Vector3> deltaPosition = new List<Vector3>();

        [HideInInspector] public Vector3 bestDeltaPosition;
        [HideInInspector] public GameObject currentLadder;

        protected bool above;

        private float acceleration;
        private float currentSpeed;
        private float runTime;
        private float deltaPositionCountDown = 1;
        private float deltaPositionCountDownCurrent = 0;

        protected override void Initialization()
        {
            base.Initialization(); //Initialize character class
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void FixedUpdate()
        {
            Movement();
            LadderMovement();
            PreviousGroundedPosition();
        }

        public virtual void Movement()
        {
            //transform.position = new Vector2(Mathf.Clamp(transform.position.x, gameManager.xMin, gameManager.xMax), Mathf.Clamp(transform.position.y, gameManager.yMin, gameManager.yMax));
            if (gameManager.gamePaused)
                return;

            if (inputManager.MovementPressed() && !character.isDead)
            {
                anim.SetBool("Moving", true);
                //Declare acceleration
                acceleration = maxSpeed / timeTillMaxSpeed; // Speed needs to ramp up to get to fullspeed. put 0 in timeTillMaxSpeed for no ramp up speed

                runTime += Time.fixedDeltaTime; //Calculate how many frames the player has been moving
                currentSpeed = inputManager.horizontalInput * acceleration * runTime;

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

        protected virtual void LadderMovement()
        {
            if(character.isOnLadder &&  currentLadder != null)
            {
                rb2d.bodyType = RigidbodyType2D.Kinematic;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);

                if(col.bounds.min.y >= (currentLadder.GetComponent<Ladder>().topOfLadder.y - col.bounds.extents.y))
                    above = true;

                else
                    above = false;

                if (inputManager.JumpHeld())
                {
                    transform.position = Vector2.MoveTowards(transform.position, currentLadder.GetComponent<Ladder>().topOfLadder, ladderSpeed * Time.deltaTime);
                    return;
                }

                if (inputManager.CrouchHeld())
                {
                    transform.position = Vector2.MoveTowards(transform.position, currentLadder.GetComponent<Ladder>().bottomOfLadder, ladderSpeed * Time.deltaTime);
                    return;
                }
            }

            else
            {
                rb2d.bodyType = RigidbodyType2D.Dynamic; 
            }
        }

        protected virtual void PreviousGroundedPosition()
        {
            if(character.isGrounded && inputManager.MovementPressed())
            {
                deltaPositionCountDownCurrent -= Time.deltaTime;

                if(deltaPositionCountDownCurrent < 0)
                {
                    if(deltaPosition.Count == 10)
                        deltaPosition.RemoveAt(0);

                    deltaPosition.Add(transform.position);
                    deltaPositionCountDownCurrent = deltaPositionCountDown;
                    bestDeltaPosition = deltaPosition[0];
                }
            }
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
            if (inputManager.SprintingHeld())
                currentSpeed *= sprintMultiplier;

            if (character.isCrouching && inputManager.CrouchHeld())
                currentSpeed *= crouchingSpeed;

            if(currentPlatform != null && (!currentPlatform.GetComponent<OneWayPlatform>() || !currentPlatform.GetComponent<Ladder>()))
            {
                if (!character.isFacingLeft && CollisionCheck(Vector2.right, .05f, jump.collisionLayer) || character.isFacingLeft && CollisionCheck(Vector2.left, .05f, jump.collisionLayer))
                    currentSpeed = .01f;
            }    
        }
    }
}

