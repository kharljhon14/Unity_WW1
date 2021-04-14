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
    }

}

