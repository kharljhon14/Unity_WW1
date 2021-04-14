using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools 
{
    public class ObjectPooling : MonoBehaviour
    {
        private static ObjectPooling instance;
        public static ObjectPooling Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject obj = new GameObject("ObjectPooler");
                    obj.AddComponent<ObjectPooling>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            else
            {
                Destroy(gameObject);
            }
        }

        private GameObject currentItem;
<<<<<<< Updated upstream
        public void CreatePool(WeaponTypes weapon, List<GameObject> currentPool, GameObject projectileParentFolder)
        {
=======

        public void CreatePool(WeaponTypes weapon, List<GameObject> currentPool, GameObject projectileParentFolder, Weapon weaponScript)
        {
            weaponScript.totalPool.Add(projectileParentFolder);
>>>>>>> Stashed changes
            for (int i = 0; i < weapon.amountToPool; i++)
            {
                currentItem = Instantiate(weapon.projectile);
                currentItem.SetActive(false);
                currentPool.Add(currentItem);
                currentItem.transform.SetParent(projectileParentFolder.transform);
            }

            projectileParentFolder.name = weapon.name;
<<<<<<< Updated upstream
        }

        public virtual GameObject GetObject(List<GameObject> currentPool)
        {
            for (int i = 0; i < currentPool.Count; i++)
            {
                if (!currentPool[i].activeInHierarchy)
                {
=======
            projectileParentFolder.tag = weapon.projectile.tag;
        }

        public virtual GameObject GetObject(List<GameObject> currentPool, WeaponTypes weapon, Weapon weaponScript , GameObject projectileParentFolder, string tag)
        {
            for (int i = 0; i < currentPool.Count; i++)
            {
                if (!currentPool[i].activeInHierarchy && currentPool[i].tag == tag )
                {
                    if(weapon.canResetPool && weaponScript.bulletsToReset.Count < weapon.amountToPool)
                    {
                        weaponScript.bulletsToReset.Add(currentPool[i]);
                    }
>>>>>>> Stashed changes
                    return currentPool[i];
                }
            }

<<<<<<< Updated upstream
=======
            foreach (GameObject item in currentPool)
            {
                if (weapon.canExpandPool && item.tag == tag)
                {
                    currentItem = Instantiate(weapon.projectile);
                    currentItem.SetActive(false);
                    currentPool.Add(currentItem);
                    currentItem.transform.SetParent(projectileParentFolder.transform);
                    return currentItem;
                }

                if (weapon.canResetPool && item.tag == tag)
                {
                    currentItem = weaponScript.bulletsToReset[0];
                    weaponScript.bulletsToReset.RemoveAt(0);
                    currentItem.SetActive(false);
                    weaponScript.bulletsToReset.Add(currentItem);
                    currentItem.GetComponent<Projectile>().DestroyProjectile();
                    return currentItem;
                }
            }

>>>>>>> Stashed changes
            return null;
        }
    }

}
