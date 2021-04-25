using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class EnemyWeapon : AIManager
    {
        [SerializeField] protected bool automatic;
        [SerializeField] protected bool onlyFireIfPlayerClose;
        [SerializeField] protected bool aimAtPlayer;
        [SerializeField] protected WeaponTypes weapon;
        [SerializeField] protected Transform projectileSpawnPosition;
        [SerializeField] protected Transform projectileSpawnRotation;
        [SerializeField] protected int automaticBurstAmount;

        [HideInInspector] public List<GameObject> currentPool;
        [HideInInspector] public List<GameObject> totalPools;
        [HideInInspector] public GameObject currentProjectile;

        protected ObjectPooling objectPooler;
        protected GameObject projectileParentFolder;
        protected bool poolSpawned;
        protected bool autoFire;
        protected float autoTime;
        protected float shotsFired;

        protected override void Initialization()
        {
            base.Initialization();
            Invoke("Pool", .05f);
        }

        protected virtual void Pool()
        {
            projectileParentFolder = new GameObject();
            objectPooler = FindObjectOfType<ObjectPooling>();
            objectPooler.CreateEnemyPool(weapon, currentPool, projectileParentFolder, this);
            timeTillAction = originalTimeTillAction;

            if (automatic)
            {
                autoTime = weapon.timeBetweenShots;
            }

            poolSpawned = true;
        }

        protected virtual void FixedUpdate()
        {
            HandleFiring();
        }

        protected virtual void HandleFiring()
        {
            if (poolSpawned)
            {
                timeTillAction -= Time.deltaTime;

                if(timeTillAction <= 0)
                {
                    if (automatic)
                    {
                        autoFire = true;
                        FireAutomaticWeapon();
                    }

                    else
                        FireWeapon();
                }
            }

            if (aimAtPlayer)
            {
                Aim();
            }
        }

        protected virtual void FireWeapon()
        {
            if(onlyFireIfPlayerClose && enemyCharacter.playerIsClose || !onlyFireIfPlayerClose)
            {
                currentProjectile = objectPooler.GetEnemyObject(currentPool, weapon, projectileParentFolder, weapon.projectile.tag);

                if (currentPool != null)
                {
                    Invoke("PlaceProjectile", .05f);
                    timeTillAction = originalTimeTillAction;
                }
                   
            }
        }

        protected virtual void FireAutomaticWeapon()
        {
            if(autoFire && onlyFireIfPlayerClose && enemyCharacter.playerIsClose || autoFire && !onlyFireIfPlayerClose)
            {
                autoTime -= Time.deltaTime;

                if(autoTime <= 0)
                {
                    currentProjectile = objectPooler.GetEnemyObject(currentPool, weapon, projectileParentFolder, weapon.projectile.tag);

                    if (currentPool != null)
                    {
                        Invoke("PlaceProjectile", .05f);
                    }

                    autoTime = weapon.timeBetweenShots;
                    shotsFired++;

                    if(shotsFired == automaticBurstAmount)
                    {
                        timeTillAction = originalTimeTillAction;
                        shotsFired = 0;
                        autoFire = false;
                    }
                }
            }
        }

        protected virtual void Aim()
        {
            if(player != null && aimAtPlayer)
            {
                Vector3 target = player.transform.position;
                target.z = 0;

                Vector3 currentPosition = projectileSpawnPosition.position;

                if (!enemyCharacter.facingLeft)
                {
                    target.x = target.x - currentPosition.x;
                    target.y = playerCollider.bounds.center.y - currentPosition.y;
                }

                else
                {
                    target.x = currentPosition.x - target.x;
                    target.y = currentPosition.y - playerCollider.bounds.center.y;
                }

                float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                projectileSpawnRotation.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        protected virtual void PlaceProjectile()
        {
            currentProjectile.transform.position = projectileSpawnPosition.position;
            currentProjectile.transform.rotation = projectileSpawnRotation.rotation;
            currentProjectile.SetActive(true);

            if (!enemyCharacter.facingLeft)
                currentProjectile.GetComponent<Projectile>().left = false;

            else
                currentProjectile.GetComponent<Projectile>().left = true;

            currentProjectile.GetComponent<Projectile>().fired = true;
        }
    }

}
