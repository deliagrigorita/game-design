using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThirdProjectile : EnemyProjectile
{
    protected override void Awake(){
        Destroy(gameObject, 5f);
        speed = 13f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Effects.Instance.PlayBossThirdAttackHit(transform.position);
        DamagePlayer(other);
        Destroy(gameObject);
    }
}
