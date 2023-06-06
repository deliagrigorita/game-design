using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootProjectile : MonoBehaviour
{
    [SerializeField] GameObject enemyProjectile;
    Vector2 direction;
    private Transform parentTrsf;

    void Start()
    {
        direction = (transform.localRotation * Vector2.right).normalized;
        parentTrsf = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnemyShoot(){
        GameObject projGameObject = Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        EnemyIceProjectile proj = projGameObject.GetComponent<EnemyIceProjectile>();

        if(proj != null){
            direction = ((transform.localRotation * parentTrsf.localRotation) * Vector2.right).normalized;
            proj.SetDirection(direction);
        }
    }
}
