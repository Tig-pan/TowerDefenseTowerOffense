using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDTO
{
    public enum State
    {
        Neutral,
        PlayerWon,
        EnemyWon
    }

    public class WinLossHandler : MonoBehaviour
    {
        public Barricade playerBase;
        public Barricade enemyBase;
        public SpriteRenderer deathFadeSprite;
        public SpriteRenderer winFadeSprite;
        public GameCoordinator coordinator;
        public AudioSource gameMusic;
        public AudioClip deathMusic;
        public AudioClip winMusic;
        public AudioClip finalBossMusic;
        public string deathScene;
        public string winScene;
        public int nextLevelIndex;
        [Space(5)]
        public bool isFinalBossFight;
        public bool hasSpawnedBoss;
        public Mob finalBossPrefab;
        public MapController enemyMap;
        public MapController playerMap;
        [Space(5)]
        public State state;

        private float fadeTimer;
        private Mob finalBoss;

        private static readonly float DeathAnimationTime = 10.5f;
        private static readonly float WinAnimationTime = 9.5f;


        private void Update()
        {
            if (state == State.Neutral && playerBase.health <= 0.0f)
            {
                state = State.EnemyWon;
                coordinator.enabled = false;
                gameMusic.clip = deathMusic;
                gameMusic.Play();
            }
            if (state == State.Neutral && enemyBase.health <= 0.0f)
            {
                if (!isFinalBossFight)
                {
                    state = State.PlayerWon;
                    coordinator.enabled = false;
                    gameMusic.clip = winMusic;
                    gameMusic.Play();

                    MainMenuManager.NextLevel = Mathf.Max(nextLevelIndex, MainMenuManager.NextLevel);
                }
                else if (!hasSpawnedBoss)
                {
                    hasSpawnedBoss = true;
                    enemyMap.SpawnTowers();
                    coordinator.SetEternalNight();

                    gameMusic.clip = finalBossMusic;
                    gameMusic.Play();

                    Mob newMob = Instantiate(finalBossPrefab);
                    newMob.transform.position = enemyBase.transform.position;
                    newMob.map = enemyMap;
                    newMob.nextMap = playerMap;
                    newMob.Init(enemyBase);

                    finalBoss = newMob;
                    enemyBase.gameObject.SetActive(false);
                }
                else
                {
                    if (finalBoss == null || finalBoss.health <= 0.0f)
                    {
                        enemyBase.gameObject.SetActive(false);

                        state = State.PlayerWon;
                        coordinator.enabled = false;
                        gameMusic.clip = winMusic;
                        gameMusic.Play();
                    }
                }
            }

            if (state == State.EnemyWon)
            {
                fadeTimer += Time.deltaTime;

                deathFadeSprite.color = new Color(0, 0, 0, fadeTimer / DeathAnimationTime);

                if (fadeTimer > DeathAnimationTime)
                {
                    SceneManager.LoadScene(deathScene);
                }
            }
            if (state == State.PlayerWon)
            {
                fadeTimer += Time.deltaTime;

                winFadeSprite.color = new Color(1, 1, 1, fadeTimer / WinAnimationTime);

                if (fadeTimer > WinAnimationTime)
                {
                    SceneManager.LoadScene(winScene);
                }
            }
        }
    }
}