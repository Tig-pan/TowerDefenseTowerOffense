using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using Unity.Burst.CompilerServices;

namespace TDTO
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        public Camera playerCamera;
        public MapController playerMap;
        public GameObject cancelPlacement;
        public GameObject enemyPlacementArea;
        public TMP_Text cancelPlacementText;
        public SpriteRenderer ghostPiece;
        public Transform ghostPieceRangeDisplay;
        public Transform rangeDisplay;
        public InfoUpgradeDisplay display;
        [Header("Tilemap")]
        public Tilemap regularPlacement;
        public Tilemap pathPlacement;
        public TileBase cannotPlaceTile;
        public TileBase canPlaceTile;
        [Header("Mana")]
        public int maxMana;
        public int mana;
        public Slider manaSlider;
        public TMP_Text manaCountText;
        public TMP_Text manaGainText;
        public float manaGainPerSecond;

        private float manaGainTimer;
        private Tilemap currentlyPlacingTilemap;
        private TowerSO currentlyPlacingTower;

        private void Update()
        {
            manaGainTimer += Time.deltaTime * manaGainPerSecond;

            if (manaGainTimer >= 1.0f)
            {
                mana = Mathf.Min(mana + 1, maxMana);
                manaGainTimer -= 1.0f;
                UpdateManaUI();
            }

            if (currentlyPlacingTower != null)
            {
                DoTowerPlacement();
            }
            else
            {
                DoTowerHoverClick();
            }

            UpdateManaUI();
        }

        void DoTowerHoverClick()
        {
            Vector2 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);

            Collider2D col = Physics2D.OverlapPoint(point);
            if (col == null)
            {
                rangeDisplay.gameObject.SetActive(false);
                return;
            }

            Tower tower = col.GetComponent<Tower>();
            if (tower == null || tower.map != playerMap)
            {
                return;
            }

            rangeDisplay.gameObject.SetActive(true);
            rangeDisplay.localScale = new Vector3(tower.rangeDisplay * 2.0f, tower.rangeDisplay * 2.0f, 1.0f);
            rangeDisplay.transform.position = col.transform.position;

            if (Input.GetMouseButtonDown(0))
            {
                display.ShowUpgrades(tower);
            }
        }

        void DoTowerPlacement()
        {
            rangeDisplay.gameObject.SetActive(false);

            Vector3 worldPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = currentlyPlacingTilemap.WorldToCell(worldPoint);

            TileBase mouseTile = currentlyPlacingTilemap.GetTile(cellPos);
            if (mouseTile == canPlaceTile)
            {
                ghostPiece.transform.position = currentlyPlacingTilemap.CellToWorld(cellPos) + currentlyPlacingTilemap.layoutGrid.cellSize * 0.5f;

                if (Input.GetMouseButtonDown(0) && mana >= currentlyPlacingTower.manaCost)
                {
                    mana -= currentlyPlacingTower.manaCost;
                    UpdateManaUI();

                    Tower newTower = Instantiate(currentlyPlacingTower.towerPrefab, playerMap.transform);
                    newTower.transform.position = ghostPiece.transform.position;
                    newTower.map = playerMap;

                    playerMap.towers.Add(newTower);
                    currentlyPlacingTilemap.SetTile(cellPos, cannotPlaceTile);

                    if (mana < currentlyPlacingTower.manaCost)
                    {
                        CancelPlacement();
                    }
                }
            }
        }

        public void CancelPlacement()
        {
            currentlyPlacingTower = null;
            UpdateCancelationUI();

            ghostPiece.gameObject.SetActive(false);

            regularPlacement.gameObject.SetActive(false);
            pathPlacement.gameObject.SetActive(false);
        }

        public void BeginPlacingTower(TowerSO tower)
        {
            if (tower == currentlyPlacingTower)
            {
                currentlyPlacingTower = null;
            }
            else
            {
                currentlyPlacingTower = tower;
                ghostPiece.sprite = tower.towerSprite;
                ghostPieceRangeDisplay.transform.localScale = new Vector3(2.0f * tower.rangeDisplaySize, 2.0f * tower.rangeDisplaySize, 1.0f);

                regularPlacement.gameObject.SetActive(!tower.placedOnTrack);
                pathPlacement.gameObject.SetActive(tower.placedOnTrack);

                currentlyPlacingTilemap = tower.placedOnTrack ? pathPlacement : regularPlacement;
            }

            UpdateCancelationUI();
        }

        private void UpdateCancelationUI()
        {
            cancelPlacement.gameObject.SetActive(currentlyPlacingTower != null);
            enemyPlacementArea.gameObject.SetActive(cancelPlacement.gameObject.activeSelf);
            ghostPiece.gameObject.SetActive(cancelPlacement.gameObject.activeSelf);

            if (cancelPlacement.gameObject.activeSelf)
            {
                cancelPlacementText.text = currentlyPlacingTower.name + " (" + currentlyPlacingTower.manaCost + " mana)";
            }
        }

        public void UpdateManaUI()
        {
            manaSlider.maxValue = maxMana;
            manaSlider.value = mana;

            manaCountText.text = "Mana: " + mana + "/" + maxMana;
            manaGainText.text = "+" + manaGainPerSecond + " mana/second\n(upgrade castle for more)";
        }
    }
}