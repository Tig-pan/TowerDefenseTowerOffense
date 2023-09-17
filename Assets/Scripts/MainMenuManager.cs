using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDTO
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject levelSelect;
        public GameObject credits;

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