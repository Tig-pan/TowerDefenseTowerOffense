using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TDTO
{
    public class TowerSelectionButton : MonoBehaviour, IPointerEnterHandler
    {
        public PlayerController playerController;
        public InfoUpgradeDisplay infoUpgradeDisplay;
        public TowerSO tower;
        public Button button;
        public bool isLocked;
        [Space(10)]
        public Image[] border;

        private static readonly Color canAfford = new Color(0.6f, 0.95f, 1.0f);
        private static readonly Color tooPoor = new Color(0.4f, 0.6f, 0.65f);

        private void Update()
        {
            if (isLocked)
            {
                button.interactable = false;
                for (int i = 0; i < border.Length; i++)
                {
                    border[i].color = tooPoor;
                }
            }
            else
            {
                bool hasEnoughMana = playerController.mana >= tower.manaCost;

                button.interactable = hasEnoughMana;
                for (int i = 0; i < border.Length; i++)
                {
                    border[i].color = hasEnoughMana ? canAfford : tooPoor;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            infoUpgradeDisplay.ShowInfo(tower);
        }
    }
}