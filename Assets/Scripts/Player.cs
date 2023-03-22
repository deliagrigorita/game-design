using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private bool isWalking;
    private Rigidbody2D playerBody;
    private Vector3 moveDir;


    private void Awake() {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        isWalking = inputVector != Vector2.zero;

        moveDir = new Vector3(inputVector.x, inputVector.y).normalized;
    }

    private void FixedUpdate() {
        playerBody.velocity = moveDir * moveSpeed; 
    }

    public bool IsWalking(){
        return isWalking; 
    }

    public Vector3 GetPosition(){
        return transform.position;
    }

    public void SetSpeed(float number){
        this.moveSpeed = number;
    }

    public void SetInitialSpeed(){
        this.moveSpeed = 10f;
    }

}
