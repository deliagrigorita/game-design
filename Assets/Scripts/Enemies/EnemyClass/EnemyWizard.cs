using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWizard : Enemy
{
    void Start()
    {
        enemyHealthSystem = new EnemyHealthSystem(100);
        enemyHealthBar.Setup(enemyHealthSystem);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

}
