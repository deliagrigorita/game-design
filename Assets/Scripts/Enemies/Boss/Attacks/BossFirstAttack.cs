using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstAttack : MonoBehaviour
{
    private float timeToShoot = 3f;
    private float waitTimeToShoot = 0f;
    private Transform player;
    private EnemyShootProjectile[] enemyShootProjectiles;

    void Start()
    {
        waitTimeToShoot = timeToShoot;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if(waitTimeToShoot <= 0){
    //         Shoot();
    //         waitTimeToShoot = timeToShoot;
    //     }
    //     waitTimeToShoot -= Time.deltaTime;
    // }

    public void Shoot(){
        HandleAiming();
        foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                Effects.Instance.PlayBossFirstAttack(gun.transform.position);
                gun.EnemyShoot();
            }
    }

    private void HandleAiming(){
        Vector3 aimDirection = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
