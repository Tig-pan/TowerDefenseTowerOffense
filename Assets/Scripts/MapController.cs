using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class MapController : MonoBehaviour
    {
        public MapController otherMap;
        public Transform[] movementWaypoints;
        public List<Tower> towers = new List<Tower>();
        public List<Mob> enemyMobs = new List<Mob>();

        public void SpawnTowers()
        {
            for (int i = 0; i < enemyMobs.Count; i++)
            {
                enemyMobs[i].Retreat(otherMap);
            }

            for (int i = 0; i < towers.Count; i++)
            {
                if (towers[i] is Barricade)
                {
                    Barricade barricade = (Barricade)towers[i];
                    barricade.health = barricade.maxHealth;
                }

                Mob newMob = Instantiate(towers[i].mobVersion);
                newMob.transform.position = towers[i].transform.position;
                newMob.map = this;
                newMob.nextMap = otherMap;
                newMob.Init(towers[i]);
                towers[i].gameObject.SetActive(false);
            }
        }
    }
}