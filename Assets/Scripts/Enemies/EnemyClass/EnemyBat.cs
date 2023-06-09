using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBat : Enemy
{
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;

     void Start()
    {
        enemyHealthSystem = new EnemyHealthSystem(50);
        enemyHealthBar.Setup(enemyHealthSystem);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

    protected override void Update(){
        if(playerObject.currentRoom != assignedRoom) {
            return;
        }
        if(Vector2.Distance(transform.position, player.position) > stoppingDistance){
            Move();
        }
        else if(Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance){
        }else if(Vector2.Distance(transform.position, player.position) < retreatDistance){
            Move();
            moveDir = -moveDir;
        }

        Shoot();
    }

    protected override void Shoot(){
        if(timeBtwShots <= 0){
            AudioManager.Instance.Play("PebbleStart");
            foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                Effects.Instance.PlayEnemyBatAttack(gun.transform.position);
                gun.EnemyShoot();
            }
            timeBtwShots = startTimeBtwShots;
        }else{
            timeBtwShots -= Time.deltaTime;
        }
    }
}
