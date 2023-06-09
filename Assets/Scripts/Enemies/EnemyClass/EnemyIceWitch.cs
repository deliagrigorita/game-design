using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIceWitch : Enemy
{
    void Start()
    {
        enemyHealthSystem = new EnemyHealthSystem(160);
        enemyHealthBar.Setup(enemyHealthSystem);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

    protected override void Shoot(){
        if(timeBtwShots <= 0){
            AudioManager.Instance.Play("IceProjectileStart");
            foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                Effects.Instance.PlayEnemyIceWitchAttack(gun.transform.position);
                gun.EnemyShoot();
            }
            timeBtwShots = startTimeBtwShots;
        }else{
            timeBtwShots -= Time.deltaTime;
        }
    }
}
