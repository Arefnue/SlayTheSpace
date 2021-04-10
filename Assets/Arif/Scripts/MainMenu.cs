using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arif.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void ExitGame()
        {
            Application.Quit();
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
