using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTO
{
    public class GameCoordinator : MonoBehaviour
    {
        public Transform timeDial;
        public SpriteRenderer darknessSprite;
        public MapController playerMap; // send at day
        public MapController enemyMap; // send at night
        public AudioSource gameMusic;
        public AudioClip nightMusic;
        public AudioClip dayMusic;

        public float currentTime = 35f; // 45 == enemy send, 15 = player send
        public float timeScale = 0.5f;

        private int enemySends = 0;
        private int playerSends = 0;

        public void SetEternalNight()
        {
            currentTime = 45.0f;

            if (timeDial != null)
            {
                timeDial.localRotation = Quaternion.Euler(0, 0, (60.0f - currentTime) * (360.0f / 60.0f));
            }
            darknessSprite.color = new Color(0, 0, 0, 0.2f + 0.2f * Mathf.Cos((currentTime - 45.0f) * (Mathf.PI / 30.0f)));

            this.enabled = false;
        }

        private void Update()
        {
            currentTime += Time.deltaTime * timeScale;
            if (currentTime >= 60.0f)
            {
                currentTime -= 60.0f;
            }

            if (currentTime >= 45.0f && (enemySends == playerSends))
            {
                enemyMap.SpawnTowers();
                enemySends++;

                if (gameMusic != null)
                {
                    gameMusic.clip = nightMusic;
                    gameMusic.Play();
                }
            }
            if (currentTime >= 15.0f && currentTime < 45.0f && (playerSends < enemySends))
            {
                playerMap.SpawnTowers();
                playerSends++;

                if (gameMusic != null)
                {
                    gameMusic.clip = dayMusic;
                    gameMusic.Play();
                }
            }

            if (timeDial != null)
            {
                timeDial.localRotation = Quaternion.Euler(0, 0, (60.0f - currentTime) * (360.0f / 60.0f));
            }
            darknessSprite.color = new Color(0, 0, 0, 0.2f + 0.2f * Mathf.Cos((currentTime - 45.0f) * (Mathf.PI / 30.0f)));

            /*if (Input.GetKeyDown(KeyCode.Minus))
            {
                playerMap.SpawnTowers();
            }
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                enemyMap.SpawnTowers();
            }*/
        }
    }
}