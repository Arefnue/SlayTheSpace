using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arif.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        
        public enum LevelState
        {
            Prepare,
            PlayerTurn,
            EnemyTurn,
            Finished
        }
        
        public int drawCount= 4;
        private LevelState _currentLevelState;
        public LevelState CurrentLevelState
        {
            get { return _currentLevelState;}
            set
            {
                switch (value)
                {
                    case LevelState.Prepare:
                        break;
                    case LevelState.PlayerTurn:
                        HandManager.instance.handController.mana = 3;
                        DrawCards(drawCount);
                        HandManager.instance.handController.canSelectCards = true;
                        break;
                    case LevelState.EnemyTurn:
                        DiscardHand();
                        currentEnemy.DoAction();
                        HandManager.instance.handController.canSelectCards = false;
                        break;
                    case LevelState.Finished:
                        HandManager.instance.handController.canSelectCards = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }

                _currentLevelState = value;
            }}
        
        
        public PlayerController playerController;
        public Transform playerPos;
        public Transform enemyPos;
        [HideInInspector]public EnemyBase currentEnemy;
        
        [HideInInspector]public List<int> drawPile = new List<int>();
        [HideInInspector]public List<int> handPile = new List<int>();
        [HideInInspector]public List<int> discardPile = new List<int>();
        
        //todo Level sistemi yap
        //todo Kartları düzenle
        
        private void Awake()
        {
            instance = this;
            CurrentLevelState = LevelState.Prepare;
        }

        private void Start()
        {
            SetGameDeck();
            CurrentLevelState = LevelState.PlayerTurn;
        }

        public void EndTurn()
        {
            CurrentLevelState = LevelState.EnemyTurn;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                DrawCards(1);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                DrawCards(3);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                EndTurn();
            }
        }

        public void SetGameDeck()
        {
            foreach (var i in GameManager.instance.myDeckIDList)
            {
                drawPile.Add(i);
            }
        }


        public void DrawCards(int drawCount)
        {
            var currentDrawCount = 0;
            for (int i = 0; i < drawCount; i++)
            {
                if (drawPile.Count<=0)
                {
                    var nDrawCount = drawCount - currentDrawCount;
                    if (nDrawCount>=discardPile.Count)
                    {
                        nDrawCount = discardPile.Count;
                    }
                    ReshuffleDiscardPile();
                    DrawCards(nDrawCount);
                    break;
                }
                else
                {
                    var randomCard = drawPile[Random.Range(0,drawPile.Count)];
                    var clone = GameManager.instance.BuildAndGetCard(randomCard);
                    HandManager.instance.handController.AddCardToHand(clone);
                    handPile.Add(randomCard);
                    drawPile.Remove(randomCard);
                    currentDrawCount++;
                    UIManager.instance.SetPileTexts();
                }
                
            }
        }

        public void OnEnemyDeath()
        {
            CurrentLevelState = LevelState.Finished;
            
        }

        public void DiscardHand()
        {
            foreach (var cardBase in HandManager.instance.handController.hand)
            {
                cardBase.Discard();
            }
            HandManager.instance.handController.hand.Clear();
        }
        
        private void ReshuffleDiscardPile()
        {
            foreach (var i in discardPile)
            {
                drawPile.Add(i);
            }
            discardPile.Clear();
        }

        public void DiscardCard(CardBase targetCard)
        {
            handPile.Remove(targetCard.myProfile.myID);
            discardPile.Add(targetCard.myProfile.myID);
            UIManager.instance.SetPileTexts();
        }
        
        
        

    }
}
