using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance{get; private set;}
    public event EventHandler OnMouseClicked;

    private PlayerInput PlayerInput;
    [SerializeField] private Camera mainCamera;
    private Vector2 mousePosition;

    private void Awake(){
        Instance = this;
        PlayerInput = new PlayerInput();
        PlayerInput.PlayerControls.Enable();
        PlayerInput.PlayerControls.Attack.performed += SendMsgMouseClicked;
        mousePosition = new Vector2(0f, 0f);
    }

    private void Update() {
        mousePosition = mainCamera.ScreenToWorldPoint(PlayerInput.PlayerControls.MousePosition.ReadValue<Vector2>());
    }

    public Vector2 GetMovementVectorNormalized(){
        Vector2 inputVector = PlayerInput.PlayerControls.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector2 GetMousePosition(){
        return mousePosition;
    }

    private void SendMsgMouseClicked(InputAction.CallbackContext context){
        OnMouseClicked?.Invoke(this, EventArgs.Empty);
    }

}
