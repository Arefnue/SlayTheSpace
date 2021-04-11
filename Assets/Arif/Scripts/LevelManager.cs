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
                        playerController.myHealth.TakeDamage(playerController.myHealth.poisonStack,true);
                        
                        playerController.myHealth.ClearBlock();
                        currentEnemy.ShowNextAction();
                        HandManager.instance.handController.canSelectCards = true;
                        break;
                    case LevelState.EnemyTurn:
                        DiscardHand();
                        currentEnemy.myHealth.TakeDamage(currentEnemy.myHealth.poisonStack,true);
                        currentEnemy.myHealth.ClearBlock();
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
        public Transform choiceParent;
        public List<Choice> choicesList;
        public bool isFinalLevel;
        public EnemyBase currentEnemy;
        [HideInInspector] public List<int> _sameChoiceContainer = new List<int>();
        
        [HideInInspector]public List<int> drawPile = new List<int>();
        [HideInInspector]public List<int> handPile = new List<int>();
        [HideInInspector]public List<int> discardPile = new List<int>();

        public Transform discardTransform;
        public Transform drawTransform;
        private void Awake()
        {
            instance = this;
            CurrentLevelState = LevelState.Prepare;
            
        }

        private void Start()
        {
           OnLevelStart();
        }

        public void EndTurn()
        {
            CurrentLevelState = LevelState.EnemyTurn;
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
                    var clone = GameManager.instance.BuildAndGetCard(randomCard,drawTransform);
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
            playerController.myHealth.SavePlayerStats();
            if (isFinalLevel)
            {
                OnFinal();
            }
            else
            {
                OnChoiceStart();
            }
            
        }

        public void OnPlayerDeath()
        {
            LoseGame();
        }

        public void IncreaseMana(int target)
        {
            HandManager.instance.handController.mana += target;
            UIManager.instance.SetPileTexts();
        }

        public void LoseGame()
        {
            CurrentLevelState = LevelState.Finished;
            DiscardHand();
            discardPile.Clear();
            drawPile.Clear();
            handPile.Clear();
            HandManager.instance.handController.hand.Clear();
            UIManager.instance.gameCanvas.SetActive(false);
            UIManager.instance.losePanel.SetActive(true);
        }

        public void OnFinal()
        {
            CurrentLevelState = LevelState.Finished;
            DiscardHand();
            discardPile.Clear();
            drawPile.Clear();
            handPile.Clear();
            HandManager.instance.handController.hand.Clear();
            UIManager.instance.gameCanvas.SetActive(false);
            UIManager.instance.winPanel.SetActive(true);
        }
        
        public void OnChoiceStart()
        {
            CurrentLevelState = LevelState.Finished;
            foreach (var choice in choicesList)
            {
                choice.DetermineChoice();
            }
            DiscardHand();
            discardPile.Clear();
            drawPile.Clear();
            handPile.Clear();
            HandManager.instance.handController.hand.Clear();
            choiceParent.gameObject.SetActive(true);
            UIManager.instance.gameCanvas.SetActive(false);
        }

        public void OnLevelStart()
        {
            if (isFinalLevel)
            {
                AudioManager.instance.PlayMusic(AudioManager.instance.bossMusic);
            }
            SetGameDeck();
            choiceParent.gameObject.SetActive(false);
            UIManager.instance.gameCanvas.SetActive(true);
            CurrentLevelState = LevelState.PlayerTurn;
        }
        
        public void DiscardHand()
        {
            foreach (var cardBase in HandManager.instance.handController.hand)
            {
                cardBase.Discard();
            }
            HandManager.instance.handController.hand.Clear();
        }

        public void ExhaustRandomCard()
        {
            var targetCard = 0;
            if (drawPile.Count>0)
            {
                targetCard = drawPile[Random.Range(0, drawPile.Count)];
                StartCoroutine(ExhaustCardRoutine(targetCard, drawTransform, currentEnemy.transform));
            }
            else if (discardPile.Count>0)
            {
                targetCard = discardPile[Random.Range(0, discardPile.Count)];
                StartCoroutine(ExhaustCardRoutine(targetCard, discardTransform, currentEnemy.transform));

            }
            else if (handPile.Count>0)
            {
                targetCard = handPile[Random.Range(0, handPile.Count)];
                CardBase tCard = HandManager.instance.handController.hand[0];
                foreach (var cardBase in HandManager.instance.handController.hand)
                {
                    if (cardBase.myProfile.myID == targetCard)
                    {
                        tCard = cardBase;
                        break;
                    }
                }

                StartCoroutine(ExhaustCardRoutine(targetCard, tCard.transform, currentEnemy.transform));
                HandManager.instance.handController.hand?.Remove(tCard);
                Destroy(tCard.gameObject);
            }
            else
            {
                LoseGame();
            }

            drawPile?.Remove(targetCard);
            handPile?.Remove(targetCard);
            discardPile?.Remove(targetCard);
            UIManager.instance.SetPileTexts();
        }

        private IEnumerator ExhaustCardRoutine(int targetID,Transform startTransform,Transform endTransform)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;

            var card = GameManager.instance.BuildAndGetCard(targetID, startTransform);
            card.transform.SetParent(endTransform);
            var startPos = card.transform.localPosition;
            var endPos = Vector3.zero;

            var startScale = card.transform.localScale;
            var endScale = Vector3.zero;
            
            var startRot = transform.localRotation;
            var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);

            
            while (true)
            {
                timer += Time.deltaTime*5;

                card.transform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                card.transform.localScale = Vector3.Lerp(startScale, endScale, timer);
                card.transform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }
            Destroy(card.gameObject);
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
