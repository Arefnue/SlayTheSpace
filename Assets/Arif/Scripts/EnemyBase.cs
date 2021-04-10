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
        
        //todo ShowNextAction
        //todo Stacklenen hasar
        //todo Block mekaniği
        //todo Güçlenme Charge
        //todo Final bossu için kart yeme mekaniği
        
        private void Awake()
        {
            myHealth = GetComponent<Health>();
            myHealth.deathAction += OnDeath;
        }

        private void Start()
        {
            LevelManager.instance.currentEnemy = this;
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
                
                yield return StartCoroutine(nameof(AttackAnim),_nextAction);
            }
            else
            {
                yield return StartCoroutine(nameof(HealAnim),_nextAction);
                
            }
            
            yield return waitTime;
            actionImage.gameObject.SetActive(true);
            LevelManager.instance.CurrentLevelState = LevelManager.LevelState.PlayerTurn;
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
        
        
    }
}