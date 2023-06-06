using System;
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
    [SerializeField] private Boss boss;

    private void Awake() {
        //Boss is not in the centre of the image, position to calculate rotation:
        posIfPlayerLeft = new Vector3(-2, 1);
        posIfPlayerRight = new Vector3(2 , 1);
        animator = GetComponent<Animator>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        parentTrsf = transform.parent;
        boss.OnAttack1 += SwordAnimate;
        boss.OnAttack2 += CastAnimate;
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

    private void SwordAnimate(object sender, EventArgs e){
        animator.Play("Attack");
    }

    private void CastAnimate(object sender, EventArgs e){
        animator.Play("Cast");
    }

}
