using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public static Particles Instance{get; private set;}
    public Transform enemyProjectileHit;

    private void Awake() {
        Instance = this;
    }

    public void PlayEnemyProjectileHit(Vector2 position){
        Instantiate(enemyProjectileHit,position,Quaternion.identity);
    }
}
