using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Arif.Scripts
{
    public class EnemyBase : MonoBehaviour
    {
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
            
            Debug.Log("DoSmt");
            yield return waitTime;
            LevelManager.instance.CurrentLevelState = LevelManager.LevelState.PlayerTurn;
        }
        private void Update()
        {
            
        }
    }
}