using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatProjectile : EnemyProjectile
{
    private void OnTriggerEnter2D(Collider2D other) {
        Effects.Instance.PlayEnemyBatHit(transform.position);
        Destroy(gameObject);
    }
}
