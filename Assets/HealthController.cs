using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HealthController : MonoBehaviour
{

    public static HealthController Instance{get; private set;}

    public int playerHealth;

    [SerializeField] private Image[] hearts;

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
          UpdateHealth();
    }

    public void UpdateHealth()
    {
         if (playerHealth <= 0)
         {
           SceneManager.LoadScene("EndScene");
         }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < playerHealth)
            {
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].color = Color.black;
            }
        }
    }

    public void Damage(int amount){
        playerHealth -= amount;
        UpdateHealth();
    }
  

}


