using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : EnemyProjectile
{
    private void OnTriggerEnter2D(Collider2D other) {
        Effects.Instance.PlayEnemyWizardHit(transform.position);
        DamagePlayer(other);
        Destroy(gameObject);
    }
}
