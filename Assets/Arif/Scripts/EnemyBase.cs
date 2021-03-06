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
        public enum EnemyActionType
        {
            Attack,
            Heal,
            Poison,
            Block,
            Space
        }

        public EnemyActionType myEnemyActionType;
        public bool targetPlayer;
        public float value;
        public Sprite actionSprite;
        public SoundProfile mySoundProfile;
    }
    public class EnemyBase : MonoBehaviour
    {
        public List<EnemyAction> myActions;
        [HideInInspector] public Health myHealth;
        
        public Image actionImage;
        public GameObject myCanvas;
        private EnemyAction _nextAction;
        public SoundProfile deathSoundProfile;
        public GameObject highlightObject;

        private void Awake()
        {
            myHealth = GetComponent<Health>();
            myHealth.deathAction += OnDeath;
            highlightObject.SetActive(false);
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
            AudioManager.instance.PlayOneShot(deathSoundProfile.GetRandomClip());
            LevelManager.instance.OnEnemyDeath();
            Destroy(gameObject);
        }
        
        private IEnumerator ActionRoutine()
        {
            var waitTime = new WaitForSeconds(1f);
            
            actionImage.gameObject.SetActive(false);
            if (_nextAction.targetPlayer)
            {
                switch (_nextAction.myEnemyActionType)
                {
                    case EnemyAction.EnemyActionType.Attack:
                        yield return StartCoroutine(nameof(AttackAnim),_nextAction);
                        break;
                    case EnemyAction.EnemyActionType.Heal:
                        break;
                    case EnemyAction.EnemyActionType.Poison:
                        yield return StartCoroutine(nameof(PoisonAnim),_nextAction);
                        break;
                    case EnemyAction.EnemyActionType.Block:
                        break;
                    case EnemyAction.EnemyActionType.Space:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
               
            }
            else
            {
                switch (_nextAction.myEnemyActionType)
                {
                    case EnemyAction.EnemyActionType.Attack:
                        break;
                    case EnemyAction.EnemyActionType.Heal:
                        yield return StartCoroutine(nameof(HealAnim),_nextAction);
                        break;
                    case EnemyAction.EnemyActionType.Poison:
                        break;
                    case EnemyAction.EnemyActionType.Block:
                        yield return StartCoroutine(nameof(BlockAnim),_nextAction);
                        break;
                    case EnemyAction.EnemyActionType.Space:
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
            AudioManager.instance.PlayOneShot(randomAction.mySoundProfile.GetRandomClip());
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
            AudioManager.instance.PlayOneShot(randomAction.mySoundProfile.GetRandomClip());
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
            AudioManager.instance.PlayOneShot(randomAction.mySoundProfile.GetRandomClip());
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
            AudioManager.instance.PlayOneShot(randomAction.mySoundProfile.GetRandomClip());
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
            AudioManager.instance.PlayOneShot(randomAction.mySoundProfile.GetRandomClip());
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