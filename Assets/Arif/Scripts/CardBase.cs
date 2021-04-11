using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Arif.Scripts
{
    public class CardBase : MonoBehaviour
    {
        [Tooltip("Mana required to use card")]
        [HideInInspector]public CardSO myProfile;
        
       
        public MeshRenderer meshRenderer;
        public Material material;

        private Vector2 _dissolveOffset = new Vector2(0.1f, 0);
        private Vector2 _dissolveSpeed = new Vector2(2f, 2f);
        private Color _dissolveColor;
        private Color _color;
        private Color _color2;

        [Header("Texts")] 
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descText;
        public TextMeshProUGUI manaText;
        public Image frontImage;
        public Image backImage;

        
        
        private bool isInactive;
        
        public void SetCard()
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            material = meshRenderer.material; // Create material instance

            _color = material.GetColor("_Color");
            _color2 = material.GetColor("_OutlineColor");
            _dissolveColor = material.GetColor("_DissolveColor");
            
            nameText.text = myProfile.myName;
            descText.text = myProfile.myDescription;
            manaText.text = myProfile.myManaCost.ToString();
            frontImage.sprite = myProfile.mySprite;
        }
        
        
        public void Use() 
        {
            foreach (var playerAction in myProfile.playerActionList)
            {
                switch (playerAction.myPlayerActionType)
                {
                    case PlayerAction.PlayerActionType.Attack:
                        break;
                    case PlayerAction.PlayerActionType.Heal:
                        LevelManager.instance.playerController.myHealth.Heal(playerAction.value);
                        break;
                    case PlayerAction.PlayerActionType.Block:
                        LevelManager.instance.playerController.myHealth.ApplyBlock(playerAction.value);
                        break;
                    case PlayerAction.PlayerActionType.IncreaseStr:
                        LevelManager.instance.playerController.IncreaseStr((int)playerAction.value);
                        break;
                    case PlayerAction.PlayerActionType.IncreaseMaxHealth:
                        GameManager.instance.ChangePlayerMaxHealth(playerAction.value);
                        break;
                    case PlayerAction.PlayerActionType.Draw:
                        LevelManager.instance.DrawCards((int)playerAction.value);
                        break;
                    case PlayerAction.PlayerActionType.ReversePoisonDamage:
                       
                        
                        break;
                    case PlayerAction.PlayerActionType.ReversePoisonHeal:
                        
                        var poisonCount = LevelManager.instance.playerController.myHealth.poisonStack;
                        LevelManager.instance.playerController.myHealth.Heal(playerAction.value*poisonCount);
                        LevelManager.instance.playerController.myHealth.ClearPoison();
                        
                        break;
                    case PlayerAction.PlayerActionType.IncreaseMana:
                        LevelManager.instance.IncreaseMana((int)playerAction.value);
                        break;
                    case PlayerAction.PlayerActionType.StealMaxHealth:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                AudioManager.instance.PlayOneShot(playerAction.mySoundProfile.GetRandomClip());
            }
            
            LevelManager.instance.DiscardCard(this);
            StartCoroutine("DiscardRoutine");
        }

        public void Attack(EnemyBase targetEnemy)
        {
            foreach (var playerAction in myProfile.playerActionList)
            {
                switch (playerAction.myPlayerActionType)
                {
                    case PlayerAction.PlayerActionType.Attack:
                        targetEnemy.myHealth.TakeDamage(playerAction.value+LevelManager.instance.playerController.bonusStr);
                        break;
                    case PlayerAction.PlayerActionType.Heal:
                        LevelManager.instance.playerController.myHealth.Heal(playerAction.value);
                        break;
                    case PlayerAction.PlayerActionType.Block:
                        break;
                    case PlayerAction.PlayerActionType.IncreaseStr:
                        break;
                    case PlayerAction.PlayerActionType.IncreaseMaxHealth:
                        break;
                    case PlayerAction.PlayerActionType.Draw:
                        break;
                    case PlayerAction.PlayerActionType.ReversePoisonDamage:
                        var poisonCount = LevelManager.instance.playerController.myHealth.poisonStack;
                        targetEnemy.myHealth.TakeDamage(playerAction.value*poisonCount);
                        LevelManager.instance.playerController.myHealth.ClearPoison();
                        break;
                    case PlayerAction.PlayerActionType.ReversePoisonHeal:
                        break;
                    case PlayerAction.PlayerActionType.IncreaseMana:
                        break;
                    case PlayerAction.PlayerActionType.StealMaxHealth:
                        targetEnemy.myHealth.DecreaseMaxHealth(playerAction.value);
                        GameManager.instance.ChangePlayerMaxHealth(playerAction.value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                AudioManager.instance.PlayOneShot(playerAction.mySoundProfile.GetRandomClip());
            }
            LevelManager.instance.DiscardCard(this);
            StartCoroutine("DiscardRoutine");
        }


        #region Actions

        

        #endregion
        
        public void Discard()
        {
            LevelManager.instance.DiscardCard(this);
            StartCoroutine("DiscardRoutine");
        }

        public void Exhaust()
        {
            StartCoroutine("Dissolve");
        }
        
        protected IEnumerator Dissolve() {
            Vector2 t = Vector2.zero - _dissolveOffset;
            while (t.x < 1) {
                t.x = (t.x + Time.deltaTime * _dissolveSpeed.x);
                if (t.y < 1) {
                    t.y = (t.y + Time.deltaTime * _dissolveSpeed.y);
                }
                material.SetVector("_Dissolve", t);
                material.SetColor("_DissolveColor", _dissolveColor * 4 * t.y);
                yield return null;
            }
            Destroy(gameObject);
        }

        private IEnumerator DiscardRoutine()
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;
            
            transform.SetParent(LevelManager.instance.discardTransform);
            
            var startPos = transform.localPosition;
            var endPos = Vector3.zero;

            var startScale = transform.localScale;
            var endScale = Vector3.zero;

            var startRot = transform.localRotation;
            var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);
            
            while (true)
            {

                timer += Time.deltaTime*5;

                transform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                transform.localScale = Vector3.Lerp(startScale, endScale, timer);
                transform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                if (timer>=1f)
                {
                    break;
                }
                
                yield return waitFrame;
            }
            
            Destroy(gameObject);
        }

        public void SetInactiveMaterialState(bool isInactive, Material inactiveMaterial = null) {
            if (isInactive == this.isInactive) {
                return; 
            }
            this.isInactive = isInactive;
            if (isInactive) {
                
                // Switch to Inactive Material
                meshRenderer.sharedMaterial = inactiveMaterial;
            } else {

                // Switch back to normal Material
                meshRenderer.sharedMaterial = material;
            }
        }
        
    }
}