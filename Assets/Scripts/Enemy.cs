using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemySpeed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private float startTimeBtwShots;
    [SerializeField] GameObject enemyProjectile;

    public int assignedRoom = int.MaxValue;

    private Transform player;
    private Player playerObject;
    private float timeBtwShots;
    private Rigidbody2D enemyBody;
    private Vector3 moveDir;
    private bool colliding;
    private float timeWait = 120;
    private bool playerWasUp;
    private bool playerWasRight;
    private Vector2 collidePoint;
    private EnemyShootProjectile[] enemyShootProjectiles;

    private void Awake() {
        enemyBody = GetComponent<Rigidbody2D>();
        timeBtwShots = startTimeBtwShots;
        colliding = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemyShootProjectiles = transform.GetComponentsInChildren<EnemyShootProjectile>();
    }

    void Update()
    {
        // if(timeWait == 0){
        //     playerWasUp = PlayerUp();
        //     playerWasRight = PlayerRight();
        //     timeWait = 15;
        // }

        if(playerObject.currentRoom != assignedRoom) {
            return;
        }

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
                    // Debug.Log("Im here...");
                    if(playerWasRight){
                        moveDir = new Vector3(1, 0).normalized;
                    }else{
                        moveDir = new Vector3(-1, 0).normalized;
                    }
                }else{
                    moveDir = new Vector3(0, 1).normalized;
                    // Debug.Log("Should be here...");
                }



                // if(timeWait > 60){
                //     moveDir = new Vector3(0, 1).normalized;
                // }else if(playerWasRight){
                //     moveDir = new Vector3(1, 0).normalized;
                // }else{
                //     moveDir = new Vector3(-1, 0).normalized;
                // }
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


                // if(timeWait > 60){
                //     moveDir = new Vector3(0, -1).normalized;
                // }else if(playerWasRight){
                //     moveDir = new Vector3(1, 0).normalized;
                // }else{
                //     moveDir = new Vector3(-1, 0).normalized;
                // }
            }
            timeWait++;
        }


        
        // if(Vector2.Distance(transform.position, player.position) > stoppingDistance){
        //     transform.position = Vector2.MoveTowards(transform.position, player.position, enemySpeed * Time.deltaTime);
        // }
        // else if(Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance){


        // }else if(Vector2.Distance(transform.position, player.position) < retreatDistance){
        //     transform.position = Vector2.MoveTowards(transform.position, player.position, -enemySpeed * Time.deltaTime);
        // }

        if(timeBtwShots <= 0){
            foreach(EnemyShootProjectile gun in enemyShootProjectiles){
                gun.EnemyShoot();
            }
            timeBtwShots = startTimeBtwShots;
        }else{
            timeBtwShots -= Time.deltaTime;
        }
        // new Vector3(transform.position.x + 10, transform.position.y, transform.position.z)
    }

    private void FixedUpdate() {
        enemyBody.velocity = moveDir * enemySpeed;
        // moveDir = new Vector3(inputVector.x, inputVector.y).normalized;
    }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     Debug.Log("Collided!");
    // }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer != gameObject.layer){
            timeWait = 60;

            colliding = true;
            foreach (ContactPoint2D hitPos in other.contacts)
            {
                collidePoint = hitPos.normal;
            }
            Debug.Log("point" + collidePoint);

        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        // Debug.Log("Colliding");
        // if(player.position.x > transform.position.x){
        //     //player is right
        //     moveDir = new Vector3(1, 0).normalized;
        //     Debug.Log("Player right");
        // }
        // else{
        //     //player is left
        //     moveDir = new Vector3(-1, 0).normalized;
        //     Debug.Log("Player left  ");
        // } 
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

    
}
