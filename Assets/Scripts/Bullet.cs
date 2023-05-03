using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDir;
    private Animation hitAnimation;
    private bool hitObject;
    [SerializeField] private float moveSpeed = 1f;

    private void Awake() {
        hitAnimation = transform.GetComponent<Animation>();
        hitObject = false;
        Destroy(gameObject, 5f);
    }

    public void Setup(Vector3 shootDir){
        this.shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(this.shootDir));
    }

    private void Update(){
        if(!hitObject){
            transform.position += shootDir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        hitObject = true;
        transform.GetComponent<BoxCollider2D>().enabled = false;

        Enemy target = collider.GetComponent<Enemy>();
        if(target != null){
            // target.Damage();
            Destroy(gameObject, 0.5f);
            hitAnimation.Play("EnemyHitLegacy");
        }else{
            Destroy(gameObject, 0.2f);
            hitAnimation.Play("ObjectHit");
        }
    }
}
