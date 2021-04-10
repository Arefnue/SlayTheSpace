using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arif.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public List<CardSO> allCardsList;
        public CardBase cardPrefab;
        public List<int> initalDeckList;
        public List<int> myDeckIDList;
        public List<CardSO> choiceCardList;
        private int _currentEnemyIndex=0;
        [HideInInspector] public List<CardBase> choiceContainer = new List<CardBase>();
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void BuildCard(int id,Transform parent)
        {
            foreach (var cardSO in allCardsList)
            {
                if (cardSO.myID == id)
                {
                    var clone = Instantiate(cardPrefab, parent);
                    clone.myProfile = cardSO;
                    clone.SetCard();
                    
                    break;
                }
            }
        }
        
        public CardBase BuildAndGetCard(int id,Transform parent)
        {
            foreach (var cardSO in allCardsList)
            {
                if (cardSO.myID == id)
                {
                    var clone = Instantiate(cardPrefab, parent);
                    clone.myProfile = cardSO;
                    clone.SetCard();
                    return clone;
                    break;
                }
            }

            return null;
        }

        public void NextLevel()
        {
           
            var i = SceneManager.GetActiveScene().buildIndex + 1;
            
            if (i>=SceneManager.sceneCountInBuildSettings)
            {
                myDeckIDList = initalDeckList;
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(i);
            }
            

        }
    }
}
