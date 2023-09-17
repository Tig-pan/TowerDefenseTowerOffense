using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class Tower : MonoBehaviour
    {
        public MapController map;
        public Mob mobVersion; // is instantiated
        public float rangeDisplay;
        public SpriteRenderer badge;
        [Space(5)]
        public Sprite leftBadge;
        public Sprite rightBadge;
        public Sprite bothBadge;
        [Space(5)]
        public TowerSO so;
        public bool leftUpgradeBought;
        public bool rightUpgradeBought;
    }
}