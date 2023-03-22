using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance{get; private set;}

    private PlayerInput PlayerInput;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator aimAnimator;
    private Vector2 mousePosition;

    private void Awake(){
        Instance = this;
        PlayerInput = new PlayerInput();
        PlayerInput.PlayerControls.Enable();
        PlayerInput.PlayerControls.Attack.performed += ShakeCam;
        // PlayerInput.PlayerControls.MousePosition.performed += OnMouseMove;
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

    void ShakeCam(InputAction.CallbackContext context){
        CinemachineShake.Instance.ShakeCamera(3f, 0.25f);
        aimAnimator.SetTrigger("Shoot");
    }

    public Vector2 GetMousePosition(){
        return mousePosition;
    }
}
