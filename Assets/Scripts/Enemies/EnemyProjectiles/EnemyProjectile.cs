using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected Transform player;
    protected Vector3 shootDir;

    protected virtual void Awake() {
        Destroy(gameObject, 5f);
        speed = 10f;
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        shootDir = (player.position - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(this.shootDir));
    }

    protected virtual void Update()
    {
        transform.position += shootDir * speed * Time.deltaTime;
        // transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        // if you want to follow player, change target to player.positon
    }

    public void SetSpeed(float newspeed){
        speed = newspeed;
    }

    public void DamagePlayer(Collider2D other){
        if(other.CompareTag("Player")){
            AudioManager.Instance.Play("PlayerHit");
            HealthController.Instance.Damage(1);
        }
    }

}
