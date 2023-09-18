using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDTO
{
    public class PauseManager : MonoBehaviour
    {
        public GameObject pause;
        public AudioSource music;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pause.activeSelf)
                {
                    Resume();
                }
                else
                {
                    Time.timeScale = 0.0f;
                    pause.gameObject.SetActive(true);
                    music.Pause();
                }
            }
        }

        public void Resume()
        {
            Time.timeScale = 1.0f;
            pause.gameObject.SetActive(false);
            music.UnPause();
        }

        public void MainMenu()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("MainMenu");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}