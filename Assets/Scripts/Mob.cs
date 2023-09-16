using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class Mob : MonoBehaviour
    {
        public int health;
        public int maxHealth;
        public float speed;
        public int armor;
        [Space(5)]
        public MapController map;
        public int currentWaypointIndex;

        public void Damage(int baseDamage)
        {
            health -= Mathf.Max(baseDamage - armor, 0);
        }

        public void Update()
        {
            Transform currentWaypoint = map.movementWaypoints[currentWaypointIndex];
        }
    }
}