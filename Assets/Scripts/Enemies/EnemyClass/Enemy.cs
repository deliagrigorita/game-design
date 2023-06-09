using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyHealthBar enemyHealthBar;
    [SerializeField] private float enemySpeed;
    [SerializeField] protected float startTimeBtwShots;

    public int assignedRoom = int.MaxValue;

    protected Player playerObject;
    protected Transform player;
    protected float timeBtwShots;
    protected Rigidbody2D enemyBody;
    protected Vector3 moveDir;
    protected bool colliding;
    protected float timeWait = 120;
    protected bool playerWasUp;
    protected bool playerWasRight;
    protected Vector2 collidePoint;
    protected EnemyShootProjectile[] enemyShootProjectiles;
    protected EnemyHealthSystem enemyHealthSystem;

    private void Awake() {
        enemyBody = GetComponent<Rigidbody2D>();
        timeBtwShots = startTimeBtwShots;
        colliding = false;
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // protected virtual void Start()
    // {
    //     enemyHealthSystem = new EnemyHealthSystem(100);
    //     enemyHealthBar.Setup(enemyHealthSystem);
    //     player = GameObject.FindGameObjectWithTag("Player").transform;
    //     enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    // }

    protected virtual void Update()
    {
        if(playerObject.currentRoom != assignedRoom) {
            return;
        }

        Move();

        Shoot();
        // new Vector3(transform.position.x + 10, transform.position.y, transform.position.z)
    }

    protected void Move(){
        if(!colliding){
            moveDir = new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized;
        }else{
            if(timeWait == 60){
                playerWasUp = PlayerUp();
                playerWasRight = PlayerRight();
                timeWait = 0;
            }
            if(playerWasUp){
                if(collidePoint.y == -1){
                    if(playerWasRight){
                        moveDir = new Vector3(1, 0).normalized;
                    }else{
                        moveDir = new Vector3(-1, 0).normalized;
                    }
                }else{
                    moveDir = new Vector3(0, 1).normalized;
                }
            }else{
                if(collidePoint.y == 1){
                    if(playerWasRight){
                        moveDir = new Vector3(1, 0).normalized;
                    }else{
                        moveDir = new Vector3(-1, 0).normalized;
                    }
                }else{
                    moveDir = new Vector3(0, -1).normalized;
                }
            }
            timeWait++;
        }
    }

    protected virtual void Shoot(){
        if(timeBtwShots <= 0){
            AudioManager.Instance.Play("FireballStart");
            foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                Effects.Instance.PlayEnemyWizardAttack(gun.transform.position);
                gun.EnemyShoot();
            }
            timeBtwShots = startTimeBtwShots;
        }else{
            timeBtwShots -= Time.deltaTime;
        }
    }

    private void FixedUpdate() {
        enemyBody.velocity = moveDir * enemySpeed;
    }


    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer != gameObject.layer){
            timeWait = 60;

            colliding = true;
            foreach (ContactPoint2D hitPos in other.contacts)
            {
                collidePoint = hitPos.normal;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
         if(other.gameObject.layer != gameObject.layer){
            colliding = false;
        }
    }

    private bool PlayerUp(){
        return player.position.y > transform.position.y;
    }

    private bool PlayerRight(){
        return player.position.x > transform.position.x;
    }

    public void Damage(){
        enemyHealthSystem.Damage(10);
        if(enemyHealthSystem.GetHealth() == 0){
            Effects.Instance.PlayEnemyDeath(transform.position);
            AudioManager.Instance.Play("EnemyDeath");
            Destroy(gameObject);
        }
    }

    
}
