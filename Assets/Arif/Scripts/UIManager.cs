using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arif.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public TextMeshProUGUI drawPileText;
        public TextMeshProUGUI discardPileText;
        public TextMeshProUGUI manaText;
        public GameObject gameCanvas;
        public GameObject winPanel;
        public GameObject losePanel;
        private void Awake()
        {
            instance = this;
            winPanel.SetActive(false);
            losePanel.SetActive(false);
        }

        public void SetPileTexts()
        {
            drawPileText.text = $"{LevelManager.instance.drawPile.Count}";
            discardPileText.text = $"{LevelManager.instance.discardPile.Count}";
            manaText.text = $"{HandManager.instance.handController.mana}";
        }

        public void EndTurn()
        {
            if (LevelManager.instance.CurrentLevelState != LevelManager.LevelState.PlayerTurn)
            {
                return;
            }
            LevelManager.instance.EndTurn();
        }

        public void MainMenu()
        {
            GameManager.instance.ResetManager();
            SceneManager.LoadScene(0);
        }
    }
}
