using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Weapon : Abilities
    {
        [SerializeField] protected List<WeaponTypes> weaponTypes;
<<<<<<< Updated upstream
        [SerializeField] protected Transform gunBarrel;
        [SerializeField] protected Transform gunRotation;

        public List<GameObject> currenPool = new List<GameObject>();
        public GameObject currentProjectile;

        private GameObject projectileParentFolder;
=======
        [SerializeField] public Transform gunBarrel;
        [SerializeField] protected Transform gunRotation;

        [HideInInspector] public List<GameObject> currenPool = new List<GameObject>();
        [HideInInspector] public List<GameObject> bulletsToReset = new List<GameObject>();
        [HideInInspector] public List<GameObject> totalPool;
        public GameObject currentProjectile;

        public WeaponTypes currentWeapon;
        private GameObject projectileParentFolder;
        private float currentTimeBetweenShots;
>>>>>>> Stashed changes

        protected override void Initialization()
        {
            base.Initialization();
<<<<<<< Updated upstream
            foreach(WeaponTypes weapon in weaponTypes)
            {
                GameObject newPool = new GameObject();
                projectileParentFolder = newPool;
                objectPooling.CreatePool(weapon, currenPool, projectileParentFolder);
            }
=======
            ChangeWeapon();
>>>>>>> Stashed changes
        }

        protected virtual void Update()
        {
            if (inputManager.WeaponFired())
            {
                FireWeapon();
            }
<<<<<<< Updated upstream
=======

            if (inputManager.ChangeWeaponPressed())
            {
                ChangeWeapon();
            }
        }
        protected virtual void FixedUpdate()
        {
            FireWeaponHeld();
>>>>>>> Stashed changes
        }

        protected virtual void FireWeapon()
        {
<<<<<<< Updated upstream
            currentProjectile = objectPooling.GetObject(currenPool);
=======
            currentProjectile = objectPooling.GetObject(currenPool, currentWeapon, this, projectileParentFolder, currentWeapon.projectile.tag);
>>>>>>> Stashed changes
            if(currentProjectile != null)
            {
                Invoke("PlaceProjectile", .1f);
            }
<<<<<<< Updated upstream
=======

            currentTimeBetweenShots = currentWeapon.timeBetweenShots;
        }

        protected virtual void FireWeaponHeld()
        {
            if (inputManager.WeaponFiredHeld())
            {
                if (currentWeapon.automatic)
                {
                    currentTimeBetweenShots = currentWeapon.lifeTime;
                    currentTimeBetweenShots -= Time.deltaTime;
                   
                    if (currentTimeBetweenShots < 0)
                    {
                        currentProjectile = objectPooling.GetObject(currenPool, currentWeapon, this, projectileParentFolder, currentWeapon.projectile.tag);
                        if (currentProjectile != null)
                        {
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
>>>>>>> Stashed changes
        }

        protected virtual void PlaceProjectile()
        {
            currentProjectile.transform.position = gunBarrel.position;
            currentProjectile.transform.rotation = gunRotation.rotation;
            currentProjectile.SetActive(true);
            if (!character.isFacingLeft)
                currentProjectile.GetComponent<Projectile>().left = false;

            else
                currentProjectile.GetComponent<Projectile>().left = true;

            currentProjectile.GetComponent<Projectile>().fired = true;
        }
    }

}
