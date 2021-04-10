using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arif.Scripts
{
    public class EnemyBase : MonoBehaviour
    {
        private void Start()
        {
            LevelManager.instance.currentEnemy = this;
        }

        public void DoAction()
        {
            StartCoroutine(nameof(ActionRoutine));

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