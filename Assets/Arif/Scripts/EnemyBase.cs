using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Arif.Scripts
{
    [Serializable]
    public class EnemyAction
    {
        public enum ActionType
        {
            Attack,
            Heal,
            Poison,
            Block,
            Space
        }

        public ActionType myActionType;
        public bool targetPlayer;
        public float value;
        public Sprite actionSprite;
    }
    public class EnemyBase : MonoBehaviour
    {
        public List<EnemyAction> myActions;
        [HideInInspector] public Health myHealth;
        
        public Image actionImage;
        public GameObject myCanvas;
        private EnemyAction _nextAction;
        
        //todo Final bossu için kart yeme mekaniği
        
        private void Awake()
        {
            myHealth = GetComponent<Health>();
            myHealth.deathAction += OnDeath;
        }
        
        public void DoAction()
        {
            StartCoroutine(nameof(ActionRoutine));

        }

        public void ShowNextAction()
        {
            _nextAction = myActions[Random.Range(0, myActions.Count)];
            actionImage.sprite = _nextAction.actionSprite;
            
        }
        
        public void OnDeath()
        {
           LevelManager.instance.OnEnemyDeath();
           Destroy(gameObject);
        }
        
        private IEnumerator ActionRoutine()
        {
            var waitTime = new WaitForSeconds(1f);
            
            actionImage.gameObject.SetActive(false);
            if (_nextAction.targetPlayer)
            {
                switch (_nextAction.myActionType)
                {
                    case EnemyAction.ActionType.Attack:
                        yield return StartCoroutine(nameof(AttackAnim),_nextAction);
                        break;
                    case EnemyAction.ActionType.Heal:
                        break;
                    case EnemyAction.ActionType.Poison:
                        yield return StartCoroutine(nameof(PoisonAnim),_nextAction);
                        break;
                    case EnemyAction.ActionType.Block:
                        break;
                    case EnemyAction.ActionType.Space:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
               
            }
            else
            {
                switch (_nextAction.myActionType)
                {
                    case EnemyAction.ActionType.Attack:
                        break;
                    case EnemyAction.ActionType.Heal:
                        yield return StartCoroutine(nameof(HealAnim),_nextAction);
                        break;
                    case EnemyAction.ActionType.Poison:
                        break;
                    case EnemyAction.ActionType.Block:
                        yield return StartCoroutine(nameof(BlockAnim),_nextAction);
                        break;
                    case EnemyAction.ActionType.Space:
                        yield return StartCoroutine(nameof(SpaceAnim),_nextAction);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                
            }
            
            yield return waitTime;
            actionImage.gameObject.SetActive(true);
            LevelManager.instance.CurrentLevelState = LevelManager.LevelState.PlayerTurn;
        }

        private IEnumerator PoisonAnim(EnemyAction randomAction)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;

            var startPos = transform.position;
            var endPos = LevelManager.instance.playerController.transform.position;

            var startRot = transform.localRotation;
            var endRot = Quaternion.Euler(60, 0, 60);
            
            while (true)
            {
                timer += Time.deltaTime*5;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                transform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }

            timer = 0f;
            
            LevelManager.instance.playerController.myHealth.ApplyPoisonDamage(randomAction.value);
            yield return new WaitForEndOfFrame();
            while (true)
            {
                timer += Time.deltaTime*5;

                transform.position = Vector3.Lerp(endPos, startPos, timer);
                transform.localRotation = Quaternion.Lerp(endRot,startRot,timer);
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }
        
        private IEnumerator AttackAnim(EnemyAction randomAction)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;

            var startPos = transform.position;
            var endPos = LevelManager.instance.playerController.transform.position;

            var startRot = transform.localRotation;
            var endRot = Quaternion.Euler(60, 0, 60);
            
            while (true)
            {
                timer += Time.deltaTime*5;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                transform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }

            timer = 0f;
            
            LevelManager.instance.playerController.myHealth.TakeDamage(randomAction.value);
            while (true)
            {
                timer += Time.deltaTime*5;

                transform.position = Vector3.Lerp(endPos, startPos, timer);
                transform.localRotation = Quaternion.Lerp(endRot,startRot,timer);
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }

        private IEnumerator HealAnim(EnemyAction randomAction)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;

            var startPos = transform.position;
            var endPos = startPos+new Vector3(0,2,0);
            
            while (true)
            {
                timer += Time.deltaTime*2;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }

            myHealth.Heal(randomAction.value);
            timer = 0f;
            while (true)
            {
                timer += Time.deltaTime*2;

                transform.position = Vector3.Lerp(endPos, startPos, timer);
                
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }
        
        private IEnumerator BlockAnim(EnemyAction randomAction)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;

            var startPos = transform.position;
            var endPos = startPos+new Vector3(0,2,0);
            
            while (true)
            {
                timer += Time.deltaTime*2;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }

            myHealth.ApplyBlock(randomAction.value);
            timer = 0f;
            while (true)
            {
                timer += Time.deltaTime*2;

                transform.position = Vector3.Lerp(endPos, startPos, timer);
                
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }
        
        private IEnumerator SpaceAnim(EnemyAction randomAction)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;

            var startPos = transform.position;
            var endPos = startPos+new Vector3(0,2,0);
            
            while (true)
            {
                timer += Time.deltaTime*2;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }

            LevelManager.instance.ExhaustRandomCard();
            timer = 0f;
            while (true)
            {
                timer += Time.deltaTime*2;

                transform.position = Vector3.Lerp(endPos, startPos, timer);
                
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }
        
    }
}