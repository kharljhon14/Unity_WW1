using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{

    public class Destroy : MonoBehaviour
    {
        [SerializeField] private float lifeTime;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
    
