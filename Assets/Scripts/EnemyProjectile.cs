using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private Transform player;
    private Vector3 shootDir;

    private void Awake() {
        Destroy(gameObject, 5f);
        speed = 12f;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        shootDir = (player.position - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(this.shootDir));
    }

    private void Update()
    {
        transform.position += shootDir * speed * Time.deltaTime;
        // transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        // if you want to follow player, change target to player.positon
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Particles.Instance.PlayEnemyProjectileHit(transform.position);
        Destroy(gameObject);

        // if(other.CompareTag("Player")){
        //     Destroy(gameObject);
        // }
        
    }
}
