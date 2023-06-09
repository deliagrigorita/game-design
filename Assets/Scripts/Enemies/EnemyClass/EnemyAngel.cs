using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAngel : Enemy
{
    private bool shooting;
    private int shootingRound;
    private float timeBtwRounds;
    [SerializeField] private float startTimeBtwRounds;

    void Start()
    {
        shootingRound = 0;
        shooting = false;
        timeBtwRounds = 0;

        enemyHealthSystem = new EnemyHealthSystem(120);
        enemyHealthBar.Setup(enemyHealthSystem);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

    protected override void Update() {
        if(playerObject.currentRoom != assignedRoom) {
            return;
        }
        Shoot();
        if(!shooting){
            Move();
        }else{
            moveDir = Vector2.zero;
        }
        
    }

    protected override void Shoot(){
        if(timeBtwShots <= 0){
            if(shootingRound == 0){
                foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                    Effects.Instance.PlayEnemyAngelAttack(gun.transform.position);
                }
                shooting = true;
            }
            ShootRound();
        }
        else{
            timeBtwShots -= Time.deltaTime;
        }
    }

    private void ShootRound(){
        if(shootingRound == 3){
            shooting = false;
            timeBtwShots = startTimeBtwShots;
            shootingRound = 0;
            timeBtwRounds = 0;
        }
        else if(timeBtwRounds <= 0){
            AudioManager.Instance.Play("AngelProjectileStart");
            foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                gun.EnemyShoot();
            }
            timeBtwRounds = startTimeBtwRounds;
            shootingRound += 1;
        }else{
            timeBtwRounds -= Time.deltaTime;
        }
    }

    public bool IsShooting(){
        return shooting;
    }
}
