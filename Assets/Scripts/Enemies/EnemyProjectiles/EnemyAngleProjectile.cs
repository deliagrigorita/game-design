using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAngleProjectile : EnemyProjectile
{
    protected override void Awake(){
        Destroy(gameObject, 5f);
        speed = 12f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Effects.Instance.PlayEnemyAngelHit(transform.position);
        Destroy(gameObject);
    }
}
