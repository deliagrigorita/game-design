using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondProjectile : BossFirstProjectile
{

    protected override void Awake(){
        Destroy(gameObject, 10f);
    }

    protected override void Update(){
        transform.position += shootDir * speed * Time.deltaTime;
    }
}
