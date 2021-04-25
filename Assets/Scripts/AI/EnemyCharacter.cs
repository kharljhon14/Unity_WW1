using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class EnemyCharacter : MonoBehaviour
    {
        [HideInInspector] public bool facingLeft;
        [HideInInspector] public bool followPlayer;
        [HideInInspector] public bool playerIsClose;

        protected Rigidbody2D rb2d;
        protected Collider2D col;
        protected EnemyMovement enemyMovement;
        protected GameObject player;
        protected Collider2D playerCollider;
        protected Animator animator;

        protected int rayHitNumber;
        public float originalTimeTillAction;
        protected float timeTillAction;

        private void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            rb2d = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            enemyMovement = GetComponent<EnemyMovement>();
            animator = GetComponent<Animator>();
            player = FindObjectOfType<Character>().gameObject;
            playerCollider = player.GetComponent<Collider2D>();
        }

        protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                //Check the hit collider by the collision mask 
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void CheckGround()
        {
            bool rightward;
            if(transform.localScale.x > 0)
                rightward = true;

            else
                rightward = false;

            Ray2D forwardRay = new Ray2D();

            if (rightward)
                forwardRay.origin = new Vector2(transform.position.x + (transform.localScale.x * .5f) + .05f, transform.position.y + (transform.localScale.y * .5f));

            else
                forwardRay.origin = new Vector2(transform.position.x + (transform.localScale.x * .5f) - .05f, transform.position.y + (transform.localScale.y * .5f));

            Ray2D[] groundRays = new Ray2D[3];

            groundRays[0].origin = new Vector2(transform.position.x - (transform.localScale.x * .5f), transform.position.y - .05f);
            groundRays[1].origin = new Vector2(transform.position.x, transform.position.y - .05f);
            groundRays[2].origin = new Vector2(transform.position.x + (transform.localScale.x * .5f), transform.position.y - .05f);
        
            if(Mathf.Round(transform.localEulerAngles.z) == 90)
            {
                groundRays[0].origin = new Vector2(transform.position.x + .05f, transform.position.y + (transform.localScale.x * .5f));
                groundRays[1].origin = new Vector2(transform.position.x + .05f, transform.position.y);
                groundRays[2].origin = new Vector2(transform.position.x + .05f, transform.position.y - (transform.localScale.x * .5f));

                if (rightward)
                    forwardRay.origin = new Vector2(transform.position.x - transform.localScale.x, transform.position.y + (transform.localScale.y * .25f) + .05f);

                else
                    forwardRay.origin = new Vector2(transform.position.x + transform.localScale.x, transform.position.y - (transform.localScale.y * .25f) - .05f);
            }

            if (Mathf.Round(transform.localEulerAngles.z) == 180)
            {
                groundRays[0].origin = new Vector2(transform.position.x - (transform.localScale.x * .5f), transform.position.y + .05f);
                groundRays[1].origin = new Vector2(transform.position.x, transform.position.y + .05f);
                groundRays[2].origin = new Vector2(transform.position.x + (transform.localScale.x * .5f), transform.position.y + .05f);

                if (rightward)
                    forwardRay.origin = new Vector2(transform.position.x - (transform.localScale.x * .5f) - .05f, transform.position.y - (transform.localScale.y * .5f));

                else
                    forwardRay.origin = new Vector2(transform.position.x - (transform.localScale.x * .5f) + .05f, transform.position.y - (transform.localScale.y * .5f));
            }

            if (Mathf.Round(transform.localEulerAngles.z) == 270)
            {
                groundRays[0].origin = new Vector2(transform.position.x - .05f, transform.position.y + (transform.localScale.x * .5f));
                groundRays[1].origin = new Vector2(transform.position.x - .05f, transform.position.y);
                groundRays[2].origin = new Vector2(transform.position.x - .05f, transform.position.y - (transform.localScale.x * .5f));

                if (rightward)
                    forwardRay.origin = new Vector2(transform.position.x + transform.localScale.x, transform.position.y + (transform.localScale.y * .25f) - .05f);

                else
                    forwardRay.origin = new Vector2(transform.position.x - transform.localScale.x, transform.position.y - (transform.localScale.y * .25f) + .05f);
            }

            RaycastHit2D forwardHit2D = new RaycastHit2D();

            if (rightward)
                forwardHit2D = Physics2D.Raycast(forwardRay.origin, transform.right, .5f);

            else
                forwardHit2D = Physics2D.Raycast(forwardRay.origin, -transform.right, .5f);

            if (forwardHit2D && forwardHit2D.collider.gameObject != player && forwardHit2D.collider.gameObject.layer != gameObject.layer)
                enemyMovement.turn = true;

            else
                enemyMovement.turn = false;

            RaycastHit2D[] hits = new RaycastHit2D[3];

            for (int i = 0; i < 3; i++)
            {
                hits[i] = Physics2D.Raycast(groundRays[i].origin, -transform.up, Mathf.Abs(transform.localScale.x * 3f));
                Debug.DrawRay(groundRays[i].origin, -transform.up * .5f, Color.red);
            }

            int numberOfHits = 0;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit)
                {
                    numberOfHits++;
                }
            }

            rayHitNumber = numberOfHits;
        }
    }


}
