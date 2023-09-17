using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class Projectile : MonoBehaviour
    {
        public MapController map;

        public GameObject splashEffect;
        public int damage = 2;
        public int maxSplashTargets = 2;
        public float splashRadius;
        public float projectileSpeed;
        public Vector3 target;

        private void Update()
        {
            float delta = projectileSpeed * Time.deltaTime;

            if (Vector3.Distance(target, transform.position) < delta)
            {
                OnHit();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, delta);
            }
        }

        public void OnHit()
        {
            if (splashEffect != null)
            {
                Instantiate(splashEffect, target, Quaternion.identity);
            }

            int hitCount = 0;
            for (int i = map.enemyMobs.Count - 1; i >= 0 && hitCount < maxSplashTargets; i--)
            {
                if (Vector3.Distance(map.enemyMobs[i].transform.position, transform.position) < splashRadius)
                {
                    map.enemyMobs[i].Damage(damage);
                    hitCount++;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
