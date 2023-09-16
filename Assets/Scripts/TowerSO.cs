using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TDTO
{
    [CreateAssetMenu(fileName="NewTower", menuName = "Scriptable Objects/Tower")]
    public class TowerSO : ScriptableObject
    {
        public string towerName;
        public string towerDescription;
        public Sprite towerSprite;
        public Tower towerPrefab;
        public int manaCost;
        public bool placedOnTrack;
    }
}