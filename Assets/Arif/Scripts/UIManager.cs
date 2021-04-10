using System;
using TMPro;
using UnityEngine;

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
            drawPileText.text = $"Draw: {LevelManager.instance.drawPile.Count}";
            discardPileText.text = $"Discard: {LevelManager.instance.discardPile.Count}";
            manaText.text = $"Mana: {HandManager.instance.handController.mana}";
        }

        public void EndTurn()
        {
            if (LevelManager.instance.CurrentLevelState != LevelManager.LevelState.PlayerTurn)
            {
                return;
            }
            LevelManager.instance.EndTurn();
        }
    }
}
