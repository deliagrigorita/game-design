using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstProjectile : EnemyProjectile
{
    private float timePassed;
    
    protected override void Awake(){
        timePassed = 0;
        Destroy(gameObject, 10f);
        speed = 2f;
    }

    protected override void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    protected override void Update(){
        transform.position += shootDir * speed * Time.deltaTime;
        
        timePassed++;
        if(timePassed > 110){
            speed = 25f;
        }else if(timePassed > 80){
            speed = 4f;
        }else if(timePassed > 50){
            speed = 2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Effects.Instance.PlayBossFirstAttackHit(transform.position);
        DamagePlayer(other);
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction){
        shootDir = new Vector3(direction.x, direction.y, 0);
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(shootDir));
    }
}
