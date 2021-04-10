using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arif.Scripts
{
    [Serializable]
    public class EnemyAction
    {
        public bool targetPlayer;
        public float value;
    }
    public class EnemyBase : MonoBehaviour
    {
        public List<EnemyAction> myActions;
        [HideInInspector] public Health myHealth;
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

       

        public void OnDeath()
        {
           
        }
        private IEnumerator ActionRoutine()
        {
            var waitTime = new WaitForSeconds(3f);
            var randomAction = myActions[Random.Range(0, myActions.Count)];

            if (randomAction.targetPlayer)
            {
                LevelManager.instance.playerController.myHealth.TakeDamage(randomAction.value);
            }
            else
            {
                myHealth.Heal(randomAction.value);
            }
            
            yield return waitTime;
            LevelManager.instance.CurrentLevelState = LevelManager.LevelState.PlayerTurn;
        }
        private void Update()
        {
            
        }
    }
}