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
            Normal,
            OnDrawing,
            Finished
        }
        
        public int drawCount= 4;
        public LevelState currentLevelState;
        public PlayerController playerController;
        
        private List<int> drawPile = new List<int>();
        private List<int> handPile = new List<int>();
        private List<int> discardPile = new List<int>();

        //todo cardDatabase
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            
            SetGameDeck();
            DrawCards(drawCount);
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
                }
                
               
            }
        }

        
        private void Drawing()
        {
            
        }
        
        private void ReshuffleDiscardPile()
        {
            drawPile = discardPile;
        }

        public void DiscardCard(CardBase targetCard)
        {
            handPile.Remove(targetCard.myProfile.myID);
            discardPile.Add(targetCard.myProfile.myID);
        }
        
        
        

    }
}
