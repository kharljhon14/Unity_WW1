using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Weapon : Abilities
    {
        [SerializeField] protected List<WeaponTypes> weaponTypes;
        [SerializeField] public Transform gunBarrel;
        [SerializeField] protected Transform gunRotation;
        [SerializeField] private GameObject particle;

        [HideInInspector] public List<GameObject> currenPool = new List<GameObject>();
        [HideInInspector] public List<GameObject> bulletsToReset = new List<GameObject>();
        [HideInInspector] public List<GameObject> totalPool;
        public GameObject currentProjectile;

        public WeaponTypes currentWeapon;
        private GameObject projectileParentFolder;
        private float currentTimeBetweenShots;

        protected override void Initialization()
        {
            base.Initialization();
            ChangeWeapon();
        }

        protected virtual void Update()
        {
            if (gameManager.gamePaused)
            {
                return;
            }

            if (inputManager.WeaponFired())
            {
                FireWeapon();
            }

            if (inputManager.ChangeWeaponPressed())
            {
                ChangeWeapon();
            }
        }
        protected virtual void FixedUpdate()
        {
            FireWeaponHeld();
        }

        protected virtual void FireWeapon()
        {
            if (!character.isDead)
            {
                currentProjectile = objectPooling.GetObject(currenPool, currentWeapon, this, projectileParentFolder, currentWeapon.projectile.tag);
                if (currentProjectile != null)
                {
                    AudioManager.instance.PlaySFX(0);
                    Invoke("PlaceProjectile", .1f);
                }

                currentTimeBetweenShots = currentWeapon.timeBetweenShots;
            }    
        }

        protected virtual void FireWeaponHeld()
        {
            if (inputManager.WeaponFiredHeld() && !character.isDead)
            {
                if (currentWeapon.automatic)
                {
                    currentTimeBetweenShots -= Time.deltaTime;
                   
                    if (currentTimeBetweenShots < 0)
                    {
                        currentProjectile = objectPooling.GetObject(currenPool, currentWeapon, this, projectileParentFolder, currentWeapon.projectile.tag);
                        if (currentProjectile != null)
                        {

                            AudioManager.instance.PlaySFX(0);
                            Invoke("PlaceProjectile", .1f);
                        }

                        currentTimeBetweenShots = currentWeapon.timeBetweenShots;
                    }
                }
            }
        }
        protected virtual void ChangeWeapon()
        {
            bool matched = new bool();

            for (int i = 0; i < weaponTypes.Count; i++)
            {
                if(currentWeapon == null)
                {
                    currentWeapon = weaponTypes[0];
                    currentTimeBetweenShots = currentWeapon.timeBetweenShots;
                    currentProjectile = currentWeapon.projectile;
                    NewPool();
                    return;
                }

                else
                {
                    if(weaponTypes[i] == currentWeapon)
                    {
                        i++;
                        if(i == weaponTypes.Count)
                        {
                            i = 0;
                        }

                        currentWeapon = weaponTypes[i];
                        currentTimeBetweenShots = currentWeapon.timeBetweenShots;
                    }
                }
            }

            for (int i = 0; i < totalPool.Count; i++)
            {
                if(currentWeapon.projectile.tag == totalPool[i].tag)
                {
                    projectileParentFolder = totalPool[i].gameObject;
                    currentProjectile = currentWeapon.projectile;
                    matched = true;
                }
            }

            if (!matched)
                NewPool();
        }

        protected virtual void NewPool()
        {
            GameObject newPool = new GameObject();
            projectileParentFolder = newPool;
            objectPooling.CreatePool(currentWeapon, currenPool, projectileParentFolder, this);
            currentProjectile = currentWeapon.projectile;

            if (currentWeapon.canResetPool)
                bulletsToReset.Clear();
        }

        protected virtual void PlaceProjectile()
        {
            currentProjectile.transform.position = gunBarrel.position;
            currentProjectile.transform.rotation = gunRotation.rotation;
            currentProjectile.SetActive(true);
            if (!character.isFacingLeft)
            {
                GameObject newEffect = Instantiate(particle, gunBarrel.position, Quaternion.identity);
                newEffect.transform.parent = transform;
                currentProjectile.GetComponent<Projectile>().left = false;
            }

            else
            {
                GameObject newEffect = Instantiate(particle, gunBarrel.position, Quaternion.Euler(0, 0, 180));
                newEffect.transform.parent = transform;
                currentProjectile.GetComponent<Projectile>().left = true;
            }
                

            currentProjectile.GetComponent<Projectile>().fired = true;
        }
    }

}
