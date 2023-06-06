using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAngelAnimator : MonoBehaviour
{
    private const string ATTACKING = "Attacking";
    private Animator animator;
    private EnemyAngel enemyAngel;

    void Start()
    {
        enemyAngel = transform.parent.gameObject.GetComponent<EnemyAngel>();
        animator = GetComponent<Animator>(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(ATTACKING, enemyAngel.IsShooting());    
    }
}
