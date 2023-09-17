using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class SlowAura : Tower
    {
        public AudioSource fireSound;
        public float pulsePeriod = 0.1f;
        public float auraRange = 1.25f;

        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > pulsePeriod)
            {
                timer = 0.0f;

                for (int i = 0; i < map.enemyMobs.Count; i++)
                {
                    float distance = Vector3.Distance(map.enemyMobs[i].transform.position, transform.position);

                    if (distance <= auraRange)
                    {
                        map.enemyMobs[i].slowTimer = pulsePeriod + 0.01f;

                        if (!fireSound.isPlaying)
                        {
                            fireSound.Play();
                        }
                    }
                }
            }
        }
    }
}