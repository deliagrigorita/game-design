using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        transform.Translate(moveSpeed * Time.deltaTime * inputVector);
    }

}
