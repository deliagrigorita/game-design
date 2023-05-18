using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private EnemyHealthSystem enemyHealthSystem;

    public void Setup(EnemyHealthSystem enemyHealthSystem){
        this.enemyHealthSystem = enemyHealthSystem;
        enemyHealthSystem.OnHealthChanged += ChangeHealthBar;
    }

    private void ChangeHealthBar(object sender, System.EventArgs e){
        transform.Find("Bar").localScale = new Vector3(2 * enemyHealthSystem.GetHealthPercent(), 0.2f);
    }
}
