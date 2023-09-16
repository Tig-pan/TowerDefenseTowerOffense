using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class MapController : MonoBehaviour
    {
        public Transform[] movementWaypoints;
        public List<Tower> towers = new List<Tower>();
    }
}