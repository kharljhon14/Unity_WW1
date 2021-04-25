using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Rotating : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        private void FixedUpdate()
        {
            transform.Rotate(new Vector3(0f, 0f, 1f) * rotationSpeed);
        }
    }
}