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
        [HideInInspector] public List<CardBase> choiceContainer = new List<CardBase>();
        public float playerCurrentHealth=100;
        public float playerMaxHealth=100;
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

        public void ResetManager()
        {
            myDeckIDList = initalDeckList;
            choiceContainer?.Clear();
            playerCurrentHealth = 100;
            playerMaxHealth = 100;
        }

        public void ChangePlayerMaxHealth(float value)
        {
            playerMaxHealth += value;
            LevelManager.instance.playerController.myHealth.maxHealth = playerMaxHealth;
            LevelManager.instance.playerController.myHealth.ChangeHealthText();
        }
        public void NextLevel()
        {
           
            var i = SceneManager.GetActiveScene().buildIndex + 1;
            
            if (i>=SceneManager.sceneCountInBuildSettings)
            {
                GameManager.instance.ResetManager();
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(i);
            }
        }
    }
}
