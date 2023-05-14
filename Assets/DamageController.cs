using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private int healthDamage;

    [SerializeField] private HealthController _healthController;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Damage();
        }
    } 
  
   void Damage()
   {
       _healthController.playerHealth = _healthController.playerHealth - healthDamage;
       _healthController.UpdateHealth();
       gameObject.SetActive(false);
    }
}
