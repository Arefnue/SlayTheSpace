using UnityEngine;

namespace Arif.Scripts
{
    [CreateAssetMenu(fileName = "Card Profile", menuName = "Playable Card", order = 0)]
    public class CardSO : ScriptableObject
    {
        #region Card Enums
        
        public enum CardRarity
        {
            Common,
            Uncommon,
            Rare
        }

        public enum CardType
        {
            Attack,
            Skill,
            Power
        }
        
        public enum CardClass
        {
            Player,
            Space,
            Curse
        }
        
        public enum UseType
        {
            Normal,
            Exhaust,
            Purge
        }

        public enum Targets
        {
            Single,
            All
        }

        #endregion

        public int myID;
        //public CardClass myClass;
        //public CardRarity myRarity;
        public CardType myType;
        //public UseType myUsage;
        //public Targets myTargets;
        public int myManaCost;
        public string myName;
        [TextArea]
        public string myDescription;
        public Sprite mySprite;
        public float damageValue;
        public float healValue;

    }
}