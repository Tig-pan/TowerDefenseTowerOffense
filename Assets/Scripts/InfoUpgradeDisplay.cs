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
        public PlayerController playerController;
        [Header("Info")]
        public TMP_Text infoName;
        public TMP_Text infoCost;
        public TMP_Text infoDescription;
        [Header("Upgrade")]
        public Sprite buyButton;
        public Sprite soldButton;
        [Space(5)]
        public Button leftUpgradeButton;
        public Image[] leftUpgradeButtonBorder;
        public TMP_Text leftUpgradeName;
        public TMP_Text leftUpgradeDescription;
        public TMP_Text leftUpgradeManaCost;
        [Space(5)]
        public Button rightUpgradeButton;
        public Image[] rightUpgradeButtonBorder;
        public TMP_Text rightUpgradeName;
        public TMP_Text rightUpgradeDescription;
        public TMP_Text rightUpgradeManaCost;

        private Tower upgradeTower;

        private static readonly Color canAfford = new Color(0.6f, 0.95f, 1.0f);
        private static readonly Color tooPoor = new Color(0.4f, 0.6f, 0.65f);

        public void ShowInfo(TowerSO tower)
        {
            infoDisplay.SetActive(true);
            upgradeDisplay.SetActive(false);

            infoName.text = tower.name + ":";
            infoCost.text = "Cost: " + tower.manaCost + " mana";
            infoDescription.text = tower.towerDescription;
        }

        private void Update()
        {
            if (upgradeDisplay.activeSelf)
            {
                bool leftActive = playerController.mana >= upgradeTower.so.leftUpgrade.manaCost && !upgradeTower.leftUpgradeBought;
                bool rightActive = playerController.mana >= upgradeTower.so.rightUpgrade.manaCost && !upgradeTower.rightUpgradeBought;

                for (int i = 0; i < leftUpgradeButtonBorder.Length; i++)
                {
                    leftUpgradeButtonBorder[i].color = leftActive ? canAfford : tooPoor;
                }
                for (int i = 0; i < rightUpgradeButtonBorder.Length; i++)
                {
                    rightUpgradeButtonBorder[i].color = rightActive ? canAfford : tooPoor;
                }
            }
        }

        public void ShowUpgrades(Tower t)
        {
            upgradeTower = t;

            infoDisplay.SetActive(false);
            upgradeDisplay.SetActive(true);

            leftUpgradeButton.image.sprite = t.leftUpgradeBought ? soldButton : buyButton;
            leftUpgradeButton.interactable = !t.leftUpgradeBought;

            leftUpgradeName.text = t.so.leftUpgrade.upgradeName;
            leftUpgradeDescription.text = t.so.leftUpgrade.upgradeDescription;
            leftUpgradeManaCost.text = t.so.leftUpgrade.manaCost + " mana";

            rightUpgradeButton.image.sprite = t.rightUpgradeBought ? soldButton : buyButton;
            rightUpgradeButton.interactable = !t.rightUpgradeBought;

            rightUpgradeName.text = t.so.rightUpgrade.upgradeName;
            rightUpgradeDescription.text = t.so.rightUpgrade.upgradeDescription;
            rightUpgradeManaCost.text = t.so.rightUpgrade.manaCost + " mana";
        }

        public void LeftUpgrade()
        {
            if (playerController.mana >= upgradeTower.so.leftUpgrade.manaCost && !upgradeTower.leftUpgradeBought)
            {
                playerController.mana -= upgradeTower.so.leftUpgrade.manaCost;
                playerController.UpdateManaUI();

                upgradeTower.leftUpgradeBought = true;

                leftUpgradeButton.image.sprite = soldButton;
                leftUpgradeButton.interactable = false;

                ApplyUpgrade(upgradeTower.so.leftUpgrade);
            }
        }

        public void RightUpgrade()
        {
            if (playerController.mana >= upgradeTower.so.rightUpgrade.manaCost && !upgradeTower.rightUpgradeBought)
            {
                playerController.mana -= upgradeTower.so.rightUpgrade.manaCost;
                playerController.UpdateManaUI();

                upgradeTower.rightUpgradeBought = true;

                rightUpgradeButton.image.sprite = soldButton;
                rightUpgradeButton.interactable = false;

                ApplyUpgrade(upgradeTower.so.rightUpgrade);
            }
        }

        private void ApplyUpgrade(Upgrade upgrade)
        {
            if (upgradeTower.map == playerController.playerMap)
            {
                playerController.manaGainPerSecond += upgrade.additionalIncome;
            }

            upgradeTower.badge.enabled = true;
            upgradeTower.badge.sprite = (upgradeTower.rightUpgradeBought && upgradeTower.leftUpgradeBought) ? upgradeTower.bothBadge : (upgradeTower.leftUpgradeBought ? upgradeTower.leftBadge : upgradeTower.rightBadge);

            upgradeTower.rangeDisplay += upgrade.additionalRange;

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