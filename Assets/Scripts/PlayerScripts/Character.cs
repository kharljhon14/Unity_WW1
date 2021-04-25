using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Character : MonoBehaviour
    {
        [HideInInspector] public bool isFacingLeft;
        [HideInInspector] public bool isJumping;
        [HideInInspector] public bool isJumpingThroughPlatform;
        [HideInInspector] public bool isGrounded;
        [HideInInspector] public bool isCrouching;
        [HideInInspector] public bool isDashing;
        [HideInInspector] public bool isWallSliding;
        [HideInInspector] public bool isOnLadder;
        [HideInInspector] public bool isDead;
        [HideInInspector] public int gameFile;

        protected Collider2D col;
        protected Rigidbody2D rb2d;
        protected Animator anim;
        protected HorizontalMovement horizontalMovement;
        protected Jump jump;
        protected InputManager inputManager;
        protected ObjectPooling objectPooling;
        protected GameObject currentPlatform;
        protected GameObject player;
        protected Weapon weapon;
        protected GameManager gameManager;
        

        private Vector2 facingLeft;

        private void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            gameFile = PlayerPrefs.GetInt("GameFile");

            col = GetComponent<Collider2D>();
            rb2d = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            horizontalMovement = GetComponent<HorizontalMovement>();
            jump = GetComponent<Jump>();
            inputManager = GetComponent<InputManager>();
            objectPooling = ObjectPooling.Instance;
            weapon = GetComponent<Weapon>();
            gameManager = FindObjectOfType<GameManager>();

            facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        protected virtual void Flip()
        {
            if (isFacingLeft)
                transform.localScale = facingLeft;

            else
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        //Check if player is grounded
        protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                //Check the hit collider by the collision mask 
                if((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    currentPlatform = hits[i].collider.gameObject;
                    return true;
                }              
            }
            return false;
        }

        public virtual bool Falling(float velocity)
        {
            if (!isGrounded && rb2d.velocity.y < velocity)
                return true;

            else
                return false;
        }

        protected virtual void FallSpeed(float speed)
        {
            //Multiply the speed of falling by this variable speed
            rb2d.velocity = new Vector2(rb2d.velocity.x, (rb2d.velocity.y) * speed);
        }

        public void InitializePlayer()
        {
            player = FindObjectOfType<Character>().gameObject;
            player.GetComponent<Character>().isFacingLeft = PlayerPrefs.GetInt(" " + gameFile + "FacingLeft") == 1 ? true : false;

            if (player.GetComponent<Character>().isFacingLeft)
            {
                player.transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
    }
}

