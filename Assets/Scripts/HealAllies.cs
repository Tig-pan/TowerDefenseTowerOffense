using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class HealAllies : MonoBehaviour
    {
        public float healPeriod = 2.0f;
        public int maxAlliesToHeal = 3;
        public int amountToHeal = 1;
        public float healRange = 1.5f;

        public Mob thisMob;

        private float timer = 0.0f;

        public void Update()
        {
            timer += Time.deltaTime;

            if (timer > healPeriod && thisMob.map == thisMob.nextMap && !thisMob.isRetreating)
            {
                int healCount = 0;
                for (int i = 0; i < thisMob.map.enemyMobs.Count && healCount < maxAlliesToHeal; i++)
                {
                    float distance = Vector3.Distance(thisMob.map.enemyMobs[i].transform.position, transform.position);

                    if (distance <= healRange && thisMob.map.enemyMobs[i].health < thisMob.map.enemyMobs[i].maxHealth)
                    {
                        thisMob.map.enemyMobs[i].health = Mathf.Min(thisMob.map.enemyMobs[i].health + amountToHeal, thisMob.map.enemyMobs[i].maxHealth);
                        healCount++;
                    }
                }

                if (healCount > 0)
                {
                    timer = 0.0f;
                }
            }
        }
    }
}