using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private bool isWalking;

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        // Debug.Log(inputVector);
        isWalking = inputVector != Vector2.zero;

        transform.Translate(moveSpeed * Time.deltaTime * inputVector);



        // Vector2 mousePosition = GameInput.Instance.GetMousePosition();
        // Vector2 direction = new Vector2(
        // mousePosition.x - transform.position.x,
        // mousePosition.y - transform.position.y
        // );

        // transform.up = direction;
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
