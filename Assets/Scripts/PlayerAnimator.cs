using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_WALKING_BACKWARDS = "IsWalkingBackwards";

    [SerializeField] private Player player;

    private Animator animator;
    private bool facingRight = true;
    private SpriteRenderer spriteRenderer;
    private Vector3 playerPosition;
    private Vector2 mousePosition;
    private Vector2 inputVector;

    private void Awake() {
        animator = GetComponent<Animator>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking()); 

        FlipSprite();

        animator.SetBool(IS_WALKING_BACKWARDS, IsWalkingBackwards());
        
    }

    private void FlipSprite(){
        playerPosition = player.GetPosition();
        mousePosition = GameInput.Instance.GetMousePosition();
        if(playerPosition.x - mousePosition.x < 0){
            if(!facingRight){
                spriteRenderer.flipX = false;
                facingRight = true;
            }
        }else if(facingRight){
            spriteRenderer.flipX = true;
            facingRight = false;
        }
    }

    private bool IsWalkingBackwards(){
        inputVector = GameInput.Instance.GetMovementVectorNormalized();

        if(facingRight){
            if(inputVector.x < 0){
                player.SetSpeed(5f);
                return true;
            }
            player.SetInitialSpeed();
            return false;
        }
        else{
            if(inputVector.x > 0){
                player.SetSpeed(5f);
                return true;
            }
            player.SetInitialSpeed();
            return false;
        }

    }


}
