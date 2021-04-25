using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class EnemyHealth : Health
    {
        [SerializeField] private GameObject deathParticles;
        [SerializeField] private int soundToPlay = 1;

        public override void DealDamage(int amount)
        {
            base.DealDamage(amount);

            AudioManager.instance.PlaySFX(soundToPlay);

            if (healthPoints <= 0 && gameObject.GetComponent<EnemyCharacter>())
            {
                if (gameObject.GetComponent<RandomDrop>())
                {
                    gameObject.GetComponent<RandomDrop>().Roll();
                }

                //gameObject.SetActive(false);
                Die();
                //Invoke("Revive", 1);
            }
        }

        protected virtual void Revive()
        {
            gameObject.GetComponent<Health>().healthPoints += 100;
            gameObject.SetActive(true);
        }

        protected virtual void Die()
        {
            AudioManager.instance.PlaySFX(2);
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}

