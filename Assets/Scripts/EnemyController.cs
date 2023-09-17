using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    [System.Serializable]
    public struct TowerOptionEntry
    {
        public Vector3 position;
        public WeightEntry[] weighting;
    }


    [System.Serializable]
    public struct WeightEntry
    {
        public TowerSO tower;
        public float weight;
    }

    public class EnemyController : MonoBehaviour
    {
        public MapController enemyMap;
        public float mana;
        public float enemyManaPerSecond;
        public float timePerDecisionAttempt;
        [Space(10)]
        public List<TowerOptionEntry> options;
        public TowerSO defaultTower;

        private float decisionTimer = 0.0f;

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < options.Count; i++)
            {
                Gizmos.DrawCube(options[i].position, 0.5f * Vector3.one);
            }
        }

        private void Update()
        {
            mana += enemyManaPerSecond * Time.deltaTime;

            decisionTimer += Time.deltaTime;

            if (decisionTimer > timePerDecisionAttempt)
            {
                AttemptDecision();
                decisionTimer = 0.0f;
            }
        }

        void AttemptDecision()
        {
            if (Random.value < 0.3f) // attempt upgrade
            {
                int index = Random.Range(0, enemyMap.towers.Count);
                Tower upgradeTower = enemyMap.towers[index];

                if (Random.value < 0.5f) // attempt left upgrade
                {
                    if (!upgradeTower.leftUpgradeBought && mana >= upgradeTower.so.leftUpgrade.manaCost)
                    {
                        mana -= upgradeTower.so.leftUpgrade.manaCost;
                        upgradeTower.leftUpgradeBought = true;
                        ApplyUpgrade(upgradeTower, upgradeTower.so.leftUpgrade);
                    }
                }
                else // attempt right upgrade
                {
                    if (!upgradeTower.rightUpgradeBought && mana >= upgradeTower.so.rightUpgrade.manaCost)
                    {
                        mana -= upgradeTower.so.rightUpgrade.manaCost;
                        upgradeTower.rightUpgradeBought = true;
                        ApplyUpgrade(upgradeTower, upgradeTower.so.rightUpgrade);
                    }
                }
            }
            else if (options.Count > 0) // attempt building
            {
                int index = Random.Range(0, options.Count);

                float weight = Random.value;
                TowerSO chosenTower = defaultTower;
                for (int i = 0; i < options[index].weighting.Length; i++)
                {
                    weight -= options[index].weighting[i].weight;
                    if (weight < 0)
                    {
                        chosenTower = options[index].weighting[i].tower;
                        break;
                    }
                }

                if (mana >= chosenTower.manaCost)
                {
                    mana -= chosenTower.manaCost;

                    Tower newTower = Instantiate(chosenTower.towerPrefab, enemyMap.transform);
                    newTower.transform.position = options[index].position;
                    newTower.map = enemyMap;
                    options.RemoveAt(index);

                    enemyMap.towers.Add(newTower);
                }
            }
        }

        private void ApplyUpgrade(Tower upgradeTower, Upgrade upgrade)
        {
            upgradeTower.rangeDisplay += upgrade.additionalRange;

            upgradeTower.badge.enabled = true;
            upgradeTower.badge.sprite = (upgradeTower.rightUpgradeBought && upgradeTower.leftUpgradeBought) ? upgradeTower.bothBadge : (upgradeTower.leftUpgradeBought ? upgradeTower.leftBadge : upgradeTower.rightBadge);

            Turret turret = upgradeTower.GetComponent<Turret>();
            if (turret != null)
            {
                turret.shotPeriod /= upgrade.fireRateMulti;
                turret.shotRange += upgrade.additionalRange;
                if (upgrade.newProjectile != null)
                {
                    turret.projectile = upgrade.newProjectile;
                }
            }

            SlowAura slow = upgradeTower.GetComponent<SlowAura>();
            if (slow != null)
            {
                slow.auraRange += upgrade.additionalRange;
            }
        }
    }
}