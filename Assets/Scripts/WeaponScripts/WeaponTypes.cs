using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    [CreateAssetMenu(fileName = "WeaponType", menuName = "WW1/Weapons", order = 1)]
    public class WeaponTypes : ScriptableObject
    {
        public GameObject projectile;
        public float projectileSpeed;
        public int amountToPool;
        public float lifeTime;
        public bool automatic;
        public float timeBetweenShots;
        public bool canExpandPool;
        public bool canResetPool;

        protected virtual void OnEnable()
        {
            if(canExpandPool && canResetPool)
            {
                canResetPool = false;
            }
        }
    }

}

