using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBurningGhoul : Enemy
{
    private float startTimeBtwDustParticles = 10;
    private float timeBtwParticles;
    [SerializeField] private float radius;
    [SerializeField] private float force;
    [SerializeField] private LayerMask hitLayerMask;

    void Start()
    {
        timeBtwParticles = startTimeBtwDustParticles;
        enemyHealthSystem = new EnemyHealthSystem(100);
        enemyHealthBar.Setup(enemyHealthSystem);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

    protected override void Update(){
        Move();
        PlayDustParticle();
    }

    private void PlayDustParticle(){
        if(timeBtwParticles < 0){
            Effects.Instance.PlayEnemyBurningGhoulWalk(new Vector2(transform.position.x, transform.position.y - 1));
            timeBtwParticles = startTimeBtwDustParticles;
        }else{
            timeBtwParticles -= 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            Explode();
        }
    }

    private void Explode(){
        Effects.Instance.PlayEnemyBurningGhoulExplode(transform.position);
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius, hitLayerMask);
        foreach (Collider2D obj in objects)
        {
            Vector2 dir = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(dir * force); 
        }
        Destroy(gameObject);
    }
}
