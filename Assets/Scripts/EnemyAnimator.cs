using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private const string PLAYER_IS_RIGHT = "PlayerIsRight";
    private const string PLAYER_IS_LEFT = "PlayerIsLeft";

    private Transform player;
    private Animator animator;
    // private bool playerIsRight;
    // private bool playerIsLeft;
    // private SpriteRenderer spriteRenderer;
    
    private void Awake() {
        animator = GetComponent<Animator>(); 
        // spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    { 
        animator.SetBool(PLAYER_IS_RIGHT, PlayerRight()); 
        animator.SetBool(PLAYER_IS_LEFT, PlayerLeft()); 
    }

    private bool PlayerLeft(){
        return player.position.x < transform.position.x;
    }

    private bool PlayerRight(){
        return player.position.x > transform.position.x;
    }
}
