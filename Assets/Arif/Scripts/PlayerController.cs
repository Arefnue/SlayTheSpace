using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arif.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public Health myHealth;
        [HideInInspector] public float bonusStr=0;

        public Image strImage;
        public TextMeshProUGUI strText;
        public GameObject playerHighlight;
        
        private void Awake()
        {
            myHealth = GetComponent<Health>();
            myHealth.deathAction += OnDeath;
            strImage.gameObject.SetActive(false);
            strText.gameObject.SetActive(false);
            playerHighlight.SetActive(false);
        }

        public void IncreaseStr(int bonus)
        {
            bonusStr += bonus;
            strImage.gameObject.SetActive(true);
            strText.gameObject.SetActive(true);
            strText.text = bonusStr.ToString();
        }

        private void OnDeath()
        {
            LevelManager.instance.OnPlayerDeath();
        }
    }
}