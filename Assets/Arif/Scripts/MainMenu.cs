using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arif.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.instance.PlayMusic(AudioManager.instance.bgMusic);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void PlayGame(int sceneIndex)
        {
            GameManager.instance.SetInitalHand();
            SceneManager.LoadScene(sceneIndex);
        }

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        
        public void SetToggle(Toggle targetToggle)
        {
            GameManager.instance.isRandomHand = targetToggle.isOn;
            Debug.Log(targetToggle.isOn);
        }
    }
}
