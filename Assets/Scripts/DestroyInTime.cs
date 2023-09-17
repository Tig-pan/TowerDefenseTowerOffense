using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class DestroyInTime : MonoBehaviour
    {
        public float timeToDestroy;

        private void Start()
        {
            Destroy(gameObject, timeToDestroy);
        }
    }
}