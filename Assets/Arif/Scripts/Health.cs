using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arif.Scripts
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        private float _currentHealth = 100;
        public bool isPlayer;
        private bool _isDead=false;
        public TextMeshProUGUI currentHealthText;
        public Image blockImage;
        public TextMeshProUGUI blockText;
        public Image poisonImage;
        public TextMeshProUGUI poisonText;
        
        public Action deathAction;

        public float poisonStack;
        public float blockStack;

        private void Start()
        {
            if (isPlayer)
            {
                _currentHealth = GameManager.instance.playerCurrentHealth;
                maxHealth = GameManager.instance.playerMaxHealth;
            }
            else
            {
                _currentHealth = maxHealth;
            }
            ClearBlock();
            ClearPoison();
            ChangeHealthText();
        }

        public void SavePlayerStats()
        {
            GameManager.instance.playerCurrentHealth = _currentHealth;
        }
        
        //todo Hasar stacklenmesi

        public void ApplyPoisonDamage(float stack)
        {
            poisonStack += stack;
            poisonImage.gameObject.SetActive(true);
            poisonText.gameObject.SetActive(true);
            poisonText.text =  $"{poisonStack}";
        }

        public void ApplyBlock(float stack)
        {
            blockStack += stack;
            ChangeHealthText();
        }
        
        public void DecreaseMaxHealth(float value)
        {
            maxHealth -= value;
            if (_currentHealth>maxHealth)
            {
                _currentHealth = maxHealth;
            }
            ChangeHealthText();
        }

        public void ClearPoison()
        {
            poisonStack = 0;
            poisonImage.gameObject.SetActive(false);
            poisonText.gameObject.SetActive(false);
        }
        public void ClearBlock()
        {
            blockStack = 0;
            blockImage.gameObject.SetActive(false);
            blockText.gameObject.SetActive(false);
        }

        public void TakeDamage(float damage,bool isPoison = false)
        {
            if (_isDead)
            {
                return;
            }

            if (!isPoison)
            {
                blockStack -= damage;
                if (blockStack<0)
                {
                    _currentHealth -= Mathf.Abs(blockStack);
                }
            }
            else
            {
                _currentHealth -= damage;
            }
            
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

        public void ChangeHealthText()
        {
            currentHealthText.text = $"{_currentHealth}/{maxHealth}";
            if (blockStack>0)
            {
                blockImage.gameObject.SetActive(true);
                blockText.gameObject.SetActive(true);
                blockText.text = $"{blockStack}";
            }
            else
            {
                blockImage.gameObject.SetActive(false);
                blockText.gameObject.SetActive(false);
            }
        }

    }
}