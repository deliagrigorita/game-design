using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Transform parentTrsf;
    private Vector3 posIfPlayerLeft;
    private Vector3 posIfPlayerRight;

    private void Awake() {
        posIfPlayerLeft = new Vector3(-2, 1);
        posIfPlayerRight = new Vector3(2 , 1);
        animator = GetComponent<Animator>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        parentTrsf = transform.parent;
    }

    void Update()
    { 
        if (PlayerLeft()){
            transform.position = parentTrsf.position + posIfPlayerLeft;
            spriteRenderer.flipX = false;
        }else{
            transform.position =  parentTrsf.position + posIfPlayerRight;
            spriteRenderer.flipX = true;
        }
    }

    private bool PlayerLeft(){
        return player.position.x < parentTrsf.position.x;
    }

    private bool PlayerRight(){
        return player.position.x > parentTrsf.position.x;
    }
}
