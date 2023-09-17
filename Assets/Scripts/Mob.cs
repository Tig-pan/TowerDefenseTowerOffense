using System;
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
        public float dps;
        public int armor;
        [Space(5)]
        public MapController map;
        public int currentWaypointIndex;
        public float currentDistanceFrac;
        public MapController nextMap;
        [Space(5)]
        public GameObject deathEffect;
        public Transform healthFill;
        [Space(5)]
        public bool isBlocked;

        private bool enteringPath;
        private Vector3 enterPathPosition;
        private float enterPathDistance;

        private GameObject originalTower;
        private Vector3 retreatPosition;
        private float retreatIndex;

        [HideInInspector()]
        public float slowTimer;
        [HideInInspector()]
        public bool isRetreating;

        private static readonly float slowSpeed = 0.6f;

        public void Init(GameObject pairedTower)
        {
            originalTower = pairedTower;
            enteringPath = true;

            enterPathDistance = 9001f;

            for (int i = 0; i < map.movementWaypoints.Length - 1; i++)
            {
                Vector3 start = map.movementWaypoints[i].position;
                Vector3 end = map.movementWaypoints[i + 1].position;

                Vector3 midpointAxis = (end - start).normalized;
                Vector3 midpointVector = start - transform.position;
                Vector3 midpointDisplacement = midpointVector - midpointAxis * Vector3.Dot(midpointVector, midpointAxis);
                Vector3 midpoint = transform.position + midpointDisplacement;

                float distanceToStart = Vector3.Distance(transform.position, start);
                float distanceToEnd = Vector3.Distance(transform.position, end);
                float distanceToMidpoint = Vector3.Distance(transform.position, midpoint);

                if (distanceToStart < enterPathDistance)
                {
                    enterPathDistance = distanceToStart;
                    enterPathPosition = start;
                    currentWaypointIndex = i;
                }
                if (distanceToEnd < enterPathDistance)
                {
                    enterPathDistance = distanceToEnd;
                    enterPathPosition = end;
                    currentWaypointIndex = i + 1;
                }
                if (distanceToMidpoint < enterPathDistance && IsPointOnLineSegment(midpoint, start, end))
                {
                    enterPathDistance = distanceToMidpoint;
                    enterPathPosition = midpoint;
                    currentWaypointIndex = i;
                }
            }

            retreatIndex = currentWaypointIndex;
            retreatPosition = transform.position;
        }

        public void Retreat(MapController mapToRetreatTo)
        {
            isRetreating = true;
            speed *= 6.0f;
            currentWaypointIndex = Mathf.Max(currentWaypointIndex - 1, 0);

            nextMap = mapToRetreatTo;
            healthFill.gameObject.SetActive(false);
        }

        bool IsPointOnLineSegment(Vector3 point, Vector3 a, Vector3 b)
        {
            Vector3 ab = b - a;
            Vector3 ac = point - a;

            float kac = Vector3.Dot(ab, ac);
            if (kac <= 0.0f)
            {
                return false;
            }

            float kab = Vector3.Dot(ab, ab);
            if (kac > kab)
            {
                return false;
            }

            return true;
        }

        public void Damage(int baseDamage)
        {
            health -= Mathf.Max(baseDamage - armor, 0);

            healthFill.localScale = new Vector3(health / (float)maxHealth, 1.0f, 1.0f);

            if (health <= 0.0f)
            {
                map.enemyMobs.Remove(this);

                if (deathEffect != null)
                {
                    Instantiate(deathEffect, transform.position, Quaternion.identity);
                }

                Destroy(this.gameObject);
                originalTower.gameObject.SetActive(true);
            }
        }

        public void Update()
        {
            if (!isBlocked)
            {
                DoMovement();
            }
        }

        private void DoMovement()
        {
            Transform currentWaypoint = map.movementWaypoints[currentWaypointIndex];
            if (currentWaypointIndex > 0)
            {
                currentDistanceFrac = currentWaypointIndex + (Vector3.Distance(transform.position, map.movementWaypoints[currentWaypointIndex - 1].position) / Vector3.Distance(map.movementWaypoints[currentWaypointIndex - 1].position, map.movementWaypoints[currentWaypointIndex].position));
            }

            slowTimer -= Time.deltaTime;

            float delta = speed * Time.deltaTime * (map != nextMap && !isRetreating ? 1.5f : 1.0f) * (slowTimer > 0.0f ? slowSpeed : 1.0f);
            if (isRetreating)
            {
                if (currentWaypointIndex > retreatIndex)
                {
                    if (Vector3.Distance(transform.position, retreatPosition) < delta)
                    {
                        Destroy(this.gameObject);
                        originalTower.gameObject.SetActive(true);

                        Barricade original = originalTower.GetComponent<Barricade>();
                        if (original)
                        {
                            original.health = original.maxHealth;
                        }
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, retreatPosition, delta);
                    }
                }
                else if (Vector3.Distance(transform.position, currentWaypoint.position) < delta)
                {
                    transform.position = currentWaypoint.position;

                    if (map == nextMap)
                    {
                        currentWaypointIndex++;
                    }
                    else
                    {
                        currentWaypointIndex--;
                        if (currentWaypointIndex < 0)
                        {
                            map.enemyMobs.Remove(this);
                            map = nextMap;
                            currentWaypointIndex = 0;
                            transform.position = map.movementWaypoints[currentWaypointIndex].position;
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, delta);
                }
            }
            else if (enteringPath)
            {
                if (Vector3.Distance(transform.position, enterPathPosition) < delta)
                {
                    transform.position = enterPathPosition;
                    enteringPath = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, enterPathPosition, delta);
                }
            }
            else if (Vector3.Distance(transform.position, currentWaypoint.position) < delta)
            {
                transform.position = currentWaypoint.position;

                if (map == nextMap)
                {
                    currentWaypointIndex = Mathf.Min(currentWaypointIndex + 1, map.movementWaypoints.Length - 1);
                }
                else
                {
                    currentWaypointIndex--;
                    if (currentWaypointIndex < 0)
                    {
                        map = nextMap;
                        currentWaypointIndex = 0;
                        transform.position = map.movementWaypoints[currentWaypointIndex].position;
                        map.enemyMobs.Add(this);
                    }
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, delta);
            }
        }
    }
}