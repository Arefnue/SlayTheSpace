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
        
        private void Awake()
        {
            instance = this;
        }

        public void SetPileTexts()
        {
            drawPileText.text = $"Draw: {LevelManager.instance.drawPile.Count}";
            discardPileText.text = $"Discard: {LevelManager.instance.discardPile.Count}";
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
