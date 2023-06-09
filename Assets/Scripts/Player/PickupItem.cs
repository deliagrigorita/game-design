using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{ 
    private Vector2 initPosition;
    private Vector2 desiredPosition;
    private float increment = 0.005f;
    private bool ascending = true;

    private void Awake() {
        initPosition = transform.position;
        desiredPosition = new Vector2(transform.position.x, transform.position.y + 1);
    }

    private void Update(){
        if(ascending){
            transform.position = new Vector3(transform.position.x, transform.position.y + increment, 0);
            if(transform.position.y >= desiredPosition.y){
                ascending = false;
            }
        }else{
            transform.position = new Vector3(transform.position.x, transform.position.y - increment, 0);
            if(transform.position.y <= initPosition.y){
                ascending = true;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            HealthController.Instance.AddHealth();
            Effects.Instance.PlayPickupHeart(transform.position);
            AudioManager.Instance.Play("Heal");
            Destroy(gameObject);
        }
    }
}
