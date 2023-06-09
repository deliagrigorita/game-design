using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float enemySpeed;
    [SerializeField] protected float startTimeBtwShots;

    //Health
    [SerializeField] private BossHealthBar bossHealthBar;
    [SerializeField] private float bossHealth;

    public int assignedRoom = int.MaxValue / 2;
    protected GameObject healthBar;

    protected GameObject healthBar;
    protected Player playerObject;
    protected Transform player;
    protected float timeBtwShots;
    protected Rigidbody2D enemyBody;
    protected Vector3 moveDir;
    protected bool colliding;
    protected float timeWait = 60;
    protected bool playerWasUp;
    protected bool playerWasRight;
    protected Vector2 collidePoint;
    // protected EnemyShootProjectile[] enemyShootProjectiles;
    protected bool startedPlayingBossTheme = false;


    //Attacks:

    //Time until attack animation stops and then can do other actions
    private float attackTime;
    //first-attack
    [SerializeField] private BossFirstAttack attack1;
    public event EventHandler OnAttack1;
    //second-attack
    [SerializeField] GameObject attack2;
    public event EventHandler OnAttack2;
    //third-attack
    [SerializeField] GameObject attack3;

    //end game script
    [SerializeField] GameObject endGame;

    private void Awake() {
        enemyBody = GetComponent<Rigidbody2D>();
        timeBtwShots = startTimeBtwShots;
        bossHealth = 2000f;
        bossHealthBar.SetMaxHealth(bossHealth);
        healthBar = GameObject.FindGameObjectWithTag("BossHealthBar");
        healthBar.SetActive(false);
        colliding = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected virtual void Update()
    {
        if(playerObject.currentRoom != assignedRoom) {
            return;
        }
        if(!startedPlayingBossTheme){
            AudioManager.Instance.Stop("MainTheme");
            AudioManager.Instance.Play("BossTheme");
            startedPlayingBossTheme = true;
        }
        Move();

        if(attackTime < 0){
            Shoot();
        }
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

    protected void Shoot(){
        if(timeBtwShots <= 0){
            float randomNumber = UnityEngine.Random.Range(0, 100);
            if(randomNumber < 40){
                OnAttack1?.Invoke(this, EventArgs.Empty);
                attack1.Shoot();
                AudioManager.Instance.Play("BossFirstAttack");
                attackTime = 40f;
            }else if(randomNumber < 60){
                OnAttack2?.Invoke(this, EventArgs.Empty);
                AudioManager.Instance.Play("BossSecondAttack");
                Invoke("SecondAttack", 0.3f);
                attackTime = 60f;
            }else{
                OnAttack2?.Invoke(this, EventArgs.Empty);
                AudioManager.Instance.Play("BossThirdAttack");
                Invoke("ThirdAttack", 0.3f);
                attackTime = 60f;
            }
            timeBtwShots = startTimeBtwShots;
        }else{
            timeBtwShots -= Time.deltaTime;
        }
    }

    private void FixedUpdate() {
        if(attackTime < 0){
            enemyBody.velocity = moveDir * enemySpeed;
        }else{
            enemyBody.velocity = Vector2.zero;
            attackTime -= 1;
        }
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

    private void SecondAttack(){
        Instantiate(attack2, new Vector2(transform.position.x, transform.position.y + 2.3f), Quaternion.identity);
    }

    private void ThirdAttack(){
        Instantiate(attack3, new Vector2(transform.position.x, transform.position.y + 2.3f), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        Bullet bullet = collider.GetComponent<Bullet>();

        if(bullet != null){
            Damage();
        }
    }

    public void Damage(){
        bossHealth -= 10;
       bossHealthBar.SetHealth(bossHealth);

        if(bossHealth <= 0){
            Effects.Instance.PlayBossDeath(transform.position);
           bossHealthBar.DestroyHealthBar();
           Instantiate(endGame, transform.position, Quaternion.identity);
           AudioManager.Instance.Stop("BossTheme");
           AudioManager.Instance.Play("BossDeath");
           AudioManager.Instance.Play("FirefliesTheme");
            Destroy(gameObject);
        }
    }

    public void ActivateBar(bool active) {
        healthBar.SetActive(active);
    }
}
