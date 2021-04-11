using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arif.Scripts
{
    [CreateAssetMenu(fileName = "Card Profile", menuName = "Playable Card", order = 0)]
    public class CardSO : ScriptableObject
    {
        #region Card Enums
        public enum CardType
        {
            Attack,
            Skill
        }
        
       

        #endregion

        public int myID;
        public CardType myType;
        public List<PlayerAction> playerActionList;
        public int myManaCost;
        public string myName;
        [TextArea]
        public string myDescription;
        public Sprite mySprite;
        // public float damageValue;
        // public float healValue;

    }

    [Serializable]
    public class PlayerAction
    {
        public enum PlayerActionType
        {
            Attack,
            Heal,
            Block,
            IncreaseStr,
            IncreaseMaxHealth,
            Draw,
            ReversePoisonDamage,
            ReversePoisonHeal,
            IncreaseMana,
            StealMaxHealth
        }

        public PlayerActionType myPlayerActionType;
        public float value;
    }
}