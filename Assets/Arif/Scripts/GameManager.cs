using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arif.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public List<CardSO> allCardsList;
        public CardBase cardPrefab;
        public List<int> myDeckIDList;
        
        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public void BuildCard(int id)
        {
            foreach (var cardSO in allCardsList)
            {
                if (cardSO.myID == id)
                {
                    var clone = Instantiate(cardPrefab, HandManager.instance.handController.transform);
                    clone.myProfile = cardSO;
                    clone.SetCard();
                    
                    break;
                }
            }
        }
        
        public CardBase BuildAndGetCard(int id)
        {
            foreach (var cardSO in allCardsList)
            {
                if (cardSO.myID == id)
                {
                    var clone = Instantiate(cardPrefab, HandManager.instance.handController.transform);
                    clone.myProfile = cardSO;
                    clone.SetCard();
                    return clone;
                    break;
                }
            }

            return null;
        }
    }
}
