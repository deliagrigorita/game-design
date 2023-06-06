using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondAttack : MonoBehaviour
{
    private EnemyShootProjectile[] enemyShootProjectiles;
    [SerializeField] private float timeToShoot = 10f;
    private float waitTimeToShoot = 0f;

    private void Awake() {
        Destroy(gameObject, 10f);
    }

    void Start()
    {
        waitTimeToShoot = timeToShoot;
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTimeToShoot < 0){
            Shoot();
            waitTimeToShoot = timeToShoot;
        }else{
            waitTimeToShoot -= 1;
        }
        
    }

    public void Shoot(){
        foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                Effects.Instance.PlayBossFirstAttack(gun.transform.position);
                gun.EnemyShoot();
            }
    }
}
