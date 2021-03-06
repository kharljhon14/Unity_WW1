using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected WeaponTypes weapon;
        [SerializeField] protected int damageAmount;
        [SerializeField] protected LayerMask damageLayer;

        public bool fired;
        public bool left;
        public float projectileLifeTime;

        private bool flipped;

        protected virtual void OnEnable()
        {
            projectileLifeTime = weapon.lifeTime;
        }

        protected virtual void FixedUpdate()
        {
            Movement();
        }

        public virtual void Movement()
        {
            if (fired)
            {
                projectileLifeTime -= Time.deltaTime;
                if(projectileLifeTime > 0)
                {
                    if (!left)
                    {
                        if (flipped)
                        {
                            flipped = false;
                            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                        }

                        transform.Translate(Vector2.right * weapon.projectileSpeed * Time.deltaTime);
                    }
                        
                    else
                    {
                        if (!flipped)
                        {
                            flipped = true;
                            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                        }

                        transform.Translate(Vector2.left * weapon.projectileSpeed * Time.deltaTime);
                    }                        
                }

                else
                    DestroyProjectile();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer & damageLayer) != 0)
            {
                if(collision.gameObject.GetComponent<Health>() != null)
                    collision.gameObject.GetComponent<Health>().DealDamage(damageAmount);

                if (collision.CompareTag("Player"))
                {
                    if(collision.transform.position.x < transform.position.x)
                        collision.gameObject.GetComponent<PlayerHealth>().left = false;

                    else
                        collision.gameObject.GetComponent<PlayerHealth>().left = true;
                }

                DestroyProjectile();
            }
        }

        public virtual void DestroyProjectile()
        {
            projectileLifeTime = weapon.lifeTime;
            gameObject.SetActive(false);
        }
    }

}
