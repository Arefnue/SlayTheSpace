using UnityEngine;

namespace Arif.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public Health myHealth;
        private void Awake()
        {
            myHealth = GetComponent<Health>();
            myHealth.deathAction += OnDeath;
        }

        private void OnDeath()
        {
            
        }
    }
}