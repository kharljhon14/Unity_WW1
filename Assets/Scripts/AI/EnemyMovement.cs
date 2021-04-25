using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class EnemyMovement : AIManager
    {
        [SerializeField] protected enum MovementType
        {
            Normal,
            HugWalls,
            Flying
        }

        [SerializeField] protected MovementType type;
        [SerializeField] protected bool spawnFacingLeft;
        [SerializeField] protected bool turnAroundOnCollision;
        [SerializeField] protected bool avoidFalling;
        [SerializeField] protected bool jump;
        public bool standStill;
        [SerializeField] protected float timeTillMaxSpeed; //Time to reach max speed
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float jumpVerticalForce;
        [SerializeField] protected float jumpHorizontalForce;
        [SerializeField] protected float minDistance;
        [SerializeField] protected LayerMask collidersToTurnAroundOn;

        [HideInInspector] public bool turn;

        private bool spawning = true;
        private float acceleration;
        private float direction;
        private float runTime;

        protected float currentSpeed;
        protected float originalWaitTime = .1f;
        protected float currentWaitTime;
        protected bool wait;
        protected bool wasJumping;

        protected override void Initialization()
        {
            base.Initialization();

            if (spawnFacingLeft)
            {
                enemyCharacter.facingLeft = true;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }

            currentWaitTime = originalWaitTime;
            timeTillAction = originalTimeTillAction;

            Invoke("Spawning", .01f);
        }

        protected virtual void Update() 
        {
            animator.SetFloat("Moving", Mathf.Abs(rb2d.velocity.x));
        }

        protected virtual void FixedUpdate()
        {
            Movement();
            CheckGround();
            EdgeOfFloor();
            HandleWait();
            Jumping();
            FollowPlayer();
            HugWalls();
        }

        protected virtual void Movement()
        {
            if(type == MovementType.Flying)
                rb2d.gravityScale = 0;

            if (!enemyCharacter.facingLeft)
            {
                direction = 1;

                if (CollisionCheck(Vector2.right, 1f, collidersToTurnAroundOn) && turnAroundOnCollision && !wasJumping && !spawning || (enemyCharacter.followPlayer && player.transform.position.x < transform.position.x))
                {
                    enemyCharacter.facingLeft = true;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

                    if (standStill)
                        rb2d.velocity = new Vector2(-jumpHorizontalForce, rb2d.velocity.y);
                }
            }

            else
            {
                direction = -1;

                if (CollisionCheck(Vector2.left, 1f, collidersToTurnAroundOn) && turnAroundOnCollision && !wasJumping && !spawning || (enemyCharacter.followPlayer && player.transform.position.x > transform.position.x))
                {
                    enemyCharacter.facingLeft = false;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

                    if (standStill)
                        rb2d.velocity = new Vector2(jumpHorizontalForce, rb2d.velocity.y);
                }
            }

            acceleration = maxSpeed / timeTillMaxSpeed; // Speed needs to ramp up to get to fullspeed. put 0 in timeTillMaxSpeed for no ramp up speed
            runTime += Time.fixedDeltaTime; //Calculate how many frames the player has been moving
            currentSpeed = direction * acceleration * runTime;
            CheckSpeed();

            if(!standStill && !enemyCharacter.followPlayer)
                rb2d.velocity = new Vector2(currentSpeed, rb2d.velocity.y);
        }

        protected virtual void FollowPlayer()
        {
            if (enemyCharacter.followPlayer)
            {
                bool toClose = new bool();

                if(Mathf.Abs(transform.position.x - player.transform.position.x) < minDistance)
                    toClose = true;

                else
                    toClose = false;

                if (!enemyCharacter.facingLeft)
                {
                    if (toClose)
                    {
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                    }                   

                    else
                    {
                        Vector2 distanceToPlayer = (new Vector3(transform.position.x - 2, transform.position.y) - player.transform.position).normalized * minDistance + player.transform.position;
                        transform.position = Vector2.MoveTowards(transform.position, distanceToPlayer, currentSpeed * Time.deltaTime);
                    }
                }

                else
                {                 

                    if (toClose)
                    {
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                    }
                       
                    else
                    {
                        Vector2 distanceToPlayer = (new Vector3(transform.position.x + 2, transform.position.y) - player.transform.position).normalized * minDistance + player.transform.position;
                        transform.position = Vector2.MoveTowards(transform.position, distanceToPlayer, -currentSpeed * Time.deltaTime);
                    }
                }
            }
        }

        protected virtual void CheckSpeed()
        {
            if (currentSpeed > maxSpeed)
                currentSpeed = maxSpeed;

            if (currentSpeed < -maxSpeed)
                currentSpeed = -maxSpeed;
        }

        protected virtual void EdgeOfFloor()
        {
            if(rayHitNumber == 1 && avoidFalling && !wait && type == MovementType.Normal)
            {
                currentWaitTime = originalWaitTime;
                wait = true;
                enemyCharacter.facingLeft = !enemyCharacter.facingLeft;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }

        protected virtual void Jumping()
        {
            if(type == MovementType.Normal)
            {
                if(rayHitNumber > 0 && jump)
                {
                    timeTillAction -= Time.deltaTime;
                    if (timeTillAction < 0)
                    {
                        rb2d.AddForce(Vector2.up * jumpVerticalForce);

                        if (!enemyCharacter.facingLeft)
                            rb2d.velocity = new Vector2(jumpHorizontalForce, rb2d.velocity.y);

                        else
                            rb2d.velocity = new Vector2(-jumpHorizontalForce, rb2d.velocity.y);
                    }
                }

                if(rayHitNumber > 0 && rb2d.velocity.y < 0)
                {
                    wasJumping = true;

                    if (standStill)
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);

                    timeTillAction = originalTimeTillAction;

                    Invoke("NoLongerInTheAir", .5f);
                }
            }
        }

        protected virtual void NoLongerInTheAir()
        {
            wasJumping = false;
        }

        protected virtual void Spawning()
        {
            spawning = false;
        }

        protected virtual void HugWalls()
        {

            if (type == MovementType.HugWalls)
            {
                turnAroundOnCollision = false;
                float newZValue = transform.localEulerAngles.z;
                rb2d.gravityScale = 0;

                if (rayHitNumber == 1 && !wait)
                {
                    wait = true;
                    currentWaitTime = originalWaitTime;
                    rb2d.velocity = Vector2.zero;
                    if (!enemyCharacter.facingLeft)
                        transform.localEulerAngles = new Vector3(0, 0, newZValue - 90);

                    else
                        transform.localEulerAngles = new Vector3(0, 0, newZValue + 90);
                }

                if (turn && !wait)
                {
                    wait = true;
                    currentWaitTime = originalWaitTime;
                    rb2d.velocity = Vector2.zero;

                    if (!enemyCharacter.facingLeft)
                    {
                        transform.localEulerAngles = new Vector3(0, 0, newZValue + 90);

                        if (Mathf.Round(transform.eulerAngles.z) == 0)
                            transform.position = new Vector2(transform.position.x, transform.position.y - (transform.localScale.x * .5f));

                        if (Mathf.Round(transform.eulerAngles.z) == 180)
                            transform.position = new Vector2(transform.position.x, transform.position.y + (transform.localScale.x * .5f));

                        if (Mathf.Round(transform.eulerAngles.z) == 90)
                            transform.position = new Vector2(transform.position.x + (transform.localScale.x * .5f), transform.position.y);

                        if (Mathf.Round(transform.eulerAngles.z) == 270)
                            transform.position = new Vector2(transform.position.x - (transform.localScale.x * .5f), transform.position.y);
                    }

                    else
                    {
                        transform.localEulerAngles = new Vector3(0, 0, newZValue - 90);

                        if (Mathf.Round(transform.eulerAngles.z) == 0)
                            transform.position = new Vector2(transform.position.x, transform.position.y + (transform.localScale.x * .5f));

                        if (Mathf.Round(transform.eulerAngles.z) == 180)
                            transform.position = new Vector2(transform.position.x, transform.position.y - (transform.localScale.x * .5f));

                        if (Mathf.Round(transform.eulerAngles.z) == 90)
                            transform.position = new Vector2(transform.position.x - (transform.localScale.x * .5f), transform.position.y);

                        if (Mathf.Round(transform.eulerAngles.z) == 270)
                            transform.position = new Vector2(transform.position.x + (transform.localScale.x * .5f), transform.position.y);
                    }
                }         

                if (Mathf.Round(transform.eulerAngles.z) == 0)
                    rb2d.velocity = new Vector2(currentSpeed, 0);

                if (Mathf.Round(transform.eulerAngles.z) == 90)
                    rb2d.velocity = new Vector2(0, currentSpeed);

                if (Mathf.Round(transform.eulerAngles.z) == 180)
                    rb2d.velocity = new Vector2(-currentSpeed, 0);

                if (Mathf.Round(transform.eulerAngles.z) == 270)
                    rb2d.velocity = new Vector2(0, -currentSpeed);

                if (rayHitNumber == 0 && !wait)
                {
                    transform.localEulerAngles = Vector3.zero;
                    rb2d.gravityScale = 1;
                }
            }
        }

        protected virtual void HandleWait()
        {
            currentWaitTime -= Time.deltaTime;

            if (currentWaitTime <= 0)
            {
                wait = false;
                currentWaitTime = 0;
            }     
        }
    }

}
