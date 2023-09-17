using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TDTO
{
    [System.Serializable]
    public class Upgrade
    {
        public string upgradeName;
        public string upgradeDescription;
        public int manaCost;
        [Space(20)]
        public float moveSpeedMulti = 1.0f;
        public float fireRateMulti = 1.0f;
        public int additionalMaxHealth = 0;
        public int additionalArmor = 0;
        public float additionalRange = 0.0f;
        public bool slowImmune;
        public Projectile newProjectile;
    }

    [CreateAssetMenu(fileName="NewTower", menuName = "Scriptable Objects/Tower")]
    public class TowerSO : ScriptableObject
    {
        public string towerName;
        public string towerDescription;
        public Sprite towerSprite;
        public Tower towerPrefab;
        public float rangeDisplaySize;
        public int manaCost;
        public bool placedOnTrack;
        [Space(10)]
        public Upgrade leftUpgrade;
        public Upgrade rightUpgrade;
    }
}