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
        public string deathScene;
        public string winScene;
        public State state;

        private float fadeTimer;

        private static readonly float DeathAnimationTime = 4f;
        private static readonly float WinAnimationTime = 6f;


        private void Update()
        {
            if (state == State.Neutral && playerBase.health <= 0.0f)
            {
                state = State.EnemyWon;
            }
            if (state == State.Neutral && enemyBase.health <= 0.0f)
            {
                state = State.PlayerWon;
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