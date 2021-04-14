using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Weapon : Abilities
    {
        [SerializeField] protected List<WeaponTypes> weaponTypes;
        [SerializeField] protected Transform gunBarrel;
        [SerializeField] protected Transform gunRotation;

        public List<GameObject> currenPool = new List<GameObject>();
        public GameObject currentProjectile;

        private GameObject projectileParentFolder;

        protected override void Initialization()
        {
            base.Initialization();
            foreach(WeaponTypes weapon in weaponTypes)
            {
                GameObject newPool = new GameObject();
                projectileParentFolder = newPool;
                objectPooling.CreatePool(weapon, currenPool, projectileParentFolder);
            }
        }

        protected virtual void Update()
        {
            if (inputManager.WeaponFired())
            {
                FireWeapon();
            }
        }

        protected virtual void FireWeapon()
        {
            currentProjectile = objectPooling.GetObject(currenPool);
            if(currentProjectile != null)
            {
                Invoke("PlaceProjectile", .1f);
            }
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
