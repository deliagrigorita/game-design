using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThirdAttack : MonoBehaviour
{
    private EnemyShootProjectile[] enemyShootProjectiles;
    [SerializeField] private float timeBetweenShoots = 10f;
    [SerializeField] private float shootsToShoot;
    private float waitTimeToShoot = 0f;
    private float shootsFired = 0;

    // private void Awake() {
    //     shootsToShoot
    // }

    void Start()
    {
        shootsToShoot = 10;
        waitTimeToShoot = timeBetweenShoots;
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shootsFired == shootsToShoot){
            Destroy(gameObject);
        }

        if(waitTimeToShoot < 0){
            Shoot();
            waitTimeToShoot = timeBetweenShoots;
        }else{
            waitTimeToShoot -= 1;
        }
        
    }

    public void Shoot(){
        foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                Effects.Instance.PlayBossThirdAttack(gun.transform.position);
                gun.EnemyShoot();
            }
        shootsFired += 1;
    }
}
