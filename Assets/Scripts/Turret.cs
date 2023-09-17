using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class Turret : Tower
    {
        public AudioSource fireSound;
        public Projectile projectile;
        public float shotPeriod;
        public float shotRange;

        private float timer;

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                Mob closest = null;
                float highestDistanceFrac = float.MinValue;

                for (int i = 0; i < map.enemyMobs.Count; i++)
                {
                    float distance = Vector3.Distance(map.enemyMobs[i].transform.position, transform.position);

                    if (distance <= shotRange && map.enemyMobs[i].currentDistanceFrac > highestDistanceFrac)
                    {
                        closest = map.enemyMobs[i];
                        highestDistanceFrac = map.enemyMobs[i].currentDistanceFrac;
                    }
                }

                if (closest != null)
                {
                    timer = shotPeriod;
                    Projectile newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                    newProjectile.target = closest.transform.position;
                    newProjectile.map = map;

                    fireSound?.Play();
                }
            }
        }
    }
}