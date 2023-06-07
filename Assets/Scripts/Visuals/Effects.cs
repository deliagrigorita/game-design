using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static Effects Instance{get; private set;}
    public Transform defaultBulletHitEnemy;
    public Transform enemyProjectileHit;
    public Transform enemyWizardAttack;
    public Transform enemyWizardHit;
    public Transform enemyIceWitchAttack;
    public Transform enemyIceWitchHit;
    public Transform enemyBurningGhoulWalk;
    public Transform enemyBurningGhoulExplosion;
    public Transform enemyAngelAttack;
    public Transform enemyAngelHit;
    public Transform enemyBatAttack;
    public Transform enemyBatHit;
    public Transform bossFirstAttack;
    public Transform bossFirstAttackHit;
    public Transform bossThirdAttack;
    public Transform bossThirdAttackHit;
    public Transform enemyDeath;
    public Transform bossDeath;
    //public Transform pickupHeart;
    //public Transform endGame;

    private void Awake() {
        Instance = this;
    }

    public void PlayDefaultBulletHitEnemy(Vector2 position){
        Instantiate(defaultBulletHitEnemy, position, Quaternion.identity);
    }

    public void PlayEnemyProjectileHit(Vector2 position){
        Instantiate(enemyProjectileHit, position, Quaternion.identity);
    }

    public void PlayEnemyWizardAttack(Vector2 position){
        Instantiate(enemyWizardAttack, position, Quaternion.identity);  
    }

    public void PlayEnemyWizardHit(Vector2 position){
        Instantiate(enemyWizardHit, position, Quaternion.identity);
    }

    public void PlayEnemyIceWitchAttack(Vector2 position){
        Instantiate(enemyIceWitchAttack, position, Quaternion.identity);
    }

    public void PlayEnemyIceWitchHit(Vector2 position){
        Instantiate(enemyIceWitchHit, position, Quaternion.identity);
    }

    public void PlayEnemyBurningGhoulWalk(Vector2 position){
        Instantiate(enemyBurningGhoulWalk, position, Quaternion.identity);
    }

    public void PlayEnemyBurningGhoulExplode(Vector2 position){
        Instantiate(enemyBurningGhoulExplosion, position, Quaternion.identity);
    }

    public void PlayEnemyAngelAttack(Vector2 position){
        Instantiate(enemyAngelAttack, position, Quaternion.identity);
    }

    public void PlayEnemyAngelHit(Vector2 position){
        Instantiate(enemyAngelHit, position, Quaternion.identity);
    }

    public void PlayEnemyBatAttack(Vector2 position){
        Instantiate(enemyBatAttack, position, Quaternion.identity);
    }

    public void PlayEnemyBatHit(Vector2 position){
        Instantiate(enemyBatHit, position, Quaternion.identity);
    }

    public void PlayBossFirstAttack(Vector2 position){
        Instantiate(bossFirstAttack, position, Quaternion.identity);
    }

    public void PlayBossFirstAttackHit(Vector2 position){
        Instantiate(bossFirstAttackHit, position, Quaternion.identity);
    }

    public void PlayBossThirdAttack(Vector2 position){
        Instantiate(bossThirdAttack, position, Quaternion.identity);
    }

    public void PlayBossThirdAttackHit(Vector2 position){
        Instantiate(bossThirdAttackHit, position, Quaternion.identity);
    }

    public void PlayEnemyDeath(Vector2 position){
        Instantiate(enemyDeath, position, Quaternion.identity);
    }

    public void PlayBossDeath(Vector2 position){
        Instantiate(bossDeath, position, Quaternion.identity);
    }

    // public void PlayPickupHeart(Vector2 position){
    //     Instantiate(pickupHeart, position, Quaternion.identity);
    // }

    // public void PlayEndGame(Vector2 position){
    //     Instantiate(endGame, position, Quaternion.identity);
    // }

}


