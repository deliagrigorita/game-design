using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance{get; private set;}

    private PlayerInput PlayerInput;

    private void Awake(){
        Instance = this;
        PlayerInput = new PlayerInput();
        PlayerInput.PlayerControls.Enable();
        PlayerInput.PlayerControls.Attack.performed += ShakeCam;
    }

    public Vector2 GetMovementVectorNormalized(){
        Vector2 inputVector = PlayerInput.PlayerControls.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }

    // public Vector2 GetPointerPosition(){
        // Vector2 mousePosition = pointerPosition.action.ReadValue<Vector2>();
        // mousePosition.z = Camera.main.nearClipPlane;
        // return Camera.main.ScreenToWorldPoint(mousePosition);
    // }

    void ShakeCam(InputAction.CallbackContext context){
        CinemachineShake.Instance.ShakeCamera(3f, 0.25f);
    }
}
