using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TDTO
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject levelSelect;
        public GameObject credits;
        [Space(5)]
        public Button level2;
        public Button level3;
        public Button level4;

        public static int NextLevel = 1;

        private void Start()
        {
            level2.interactable = NextLevel >= 2;
            level3.interactable = NextLevel >= 3;
            level4.interactable = NextLevel >= 4;
        }

        public void Play()
        {
            switch (NextLevel)
            {
                case 1:
                    LoadLevel("Level1");
                    break;
                case 2:
                    LoadLevel("Level2");
                    break;
                case 3:
                    LoadLevel("Level3");
                    break;
                default:
                    LoadLevel("Level4");
                    break;
            }
        }

        public void MainMenu()
        {
            mainMenu.SetActive(true);
            levelSelect.SetActive(false);
            credits.SetActive(false);
        }

        public void LevelSelect()
        {
            mainMenu.SetActive(false);
            levelSelect.SetActive(true);
            credits.SetActive(false);
        }

        public void Credits()
        {
            mainMenu.SetActive(false);
            levelSelect.SetActive(false);
            credits.SetActive(true);
        }

        public void LoadLevel(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}