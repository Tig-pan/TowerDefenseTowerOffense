using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TDTO
{
    public class InfoUpgradeDisplay : MonoBehaviour
    {
        public GameObject infoDisplay;
        public GameObject upgradeDisplay;
        [Header("Info")]
        public TMP_Text infoName;
        public TMP_Text infoCost;
        public TMP_Text infoDescription;

        public void ShowInfo(TowerSO tower)
        {
            infoDisplay.SetActive(true);
            upgradeDisplay.SetActive(false);

            infoName.text = tower.name + ":";
            infoCost.text = "Cost: " + tower.manaCost + " mana";
            infoDescription.text = tower.towerDescription;
        }
    }
}