using System;
using TMPro;
using UnityEngine;

namespace Arif.Scripts
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        private float _currentHealth;
        
        private bool _isDead=false;
        public TextMeshProUGUI currentHealthText;
        public Action deathAction;

        private void Start()
        {
            _currentHealth = maxHealth;
            ChangeHealthText();
        }

        public void TakeDamage(float damage)
        {
            if (_isDead)
            {
                return;
            }
            _currentHealth -= damage;
            if (_currentHealth<=0)
            {
                _currentHealth = 0;
                _isDead = true;
                deathAction?.Invoke();
            }
            ChangeHealthText();
        }
        
        public void Heal(float healValue)
        {
            if (_isDead)
            {
                return;
            }
            _currentHealth += healValue;
            if (_currentHealth>maxHealth)
            {
                _currentHealth = maxHealth;
            }
            ChangeHealthText();
        }

        private void ChangeHealthText()
        {
            currentHealthText.text = $"{_currentHealth}/{maxHealth}";
        }

    }
}