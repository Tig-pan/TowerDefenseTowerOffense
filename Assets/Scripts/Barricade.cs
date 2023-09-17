using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class Barricade : Tower
    {
        public float health;
        public float maxHealth;
        public bool isFinalCastle;
        [Space(5)]
        public Transform healthFill;

        private static readonly float DefaultBlockingDistance = 0.5f;
        private static readonly float BlockingDistancePerBlocked = 0.2f;

        private List<Mob> currentlyBlocking = new List<Mob>();

        public void Update()
        {
            if (health > 0)
            {
                for (int i = currentlyBlocking.Count - 1; i >= 0; i--)
                {
                    if (currentlyBlocking[i] == null || currentlyBlocking[i].health < 0)
                    {
                        currentlyBlocking.RemoveAt(i);
                    }
                    else
                    {
                        int healthBefore = (int)health;
                        health -= currentlyBlocking[i].dps * Time.deltaTime;
                        int healthAfter = (int)health;

                        if (leftUpgradeBought && healthAfter != healthBefore) // spikes/thorns
                        {
                            currentlyBlocking[i].Damage(healthBefore - healthAfter, true);
                        }

                        healthFill.transform.localScale = new Vector3(health / maxHealth, 1, 1);

                        if (health <= 0)
                        {
                            OnDeath();
                            return;
                        }
                    }
                }

                float blockingDistance = DefaultBlockingDistance + BlockingDistancePerBlocked * currentlyBlocking.Count;

                for (int i = 0; i < map.enemyMobs.Count; i++)
                {
                    Mob mob = map.enemyMobs[i];
                    if (!mob.isBlocked && Vector3.Distance(mob.transform.position, transform.position) < blockingDistance)
                    {
                        mob.isBlocked = true;
                        currentlyBlocking.Add(mob);
                    }
                }
            }
        }

        void OnDeath()
        {
            healthFill.gameObject.SetActive(false);
            for (int i = 0; i < currentlyBlocking.Count; i++)
            {
                currentlyBlocking[i].isBlocked = false;
            }
        }
    }
}