using DefaultNamespace;
using Handler;
using Manager;
using RestSharp;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Manager { get; private set; }

    public bool MovementLock { get; set; }
    private Vector2 p1MoveDirection;
    private Vector2 p2MoveDirection;

    void Start()
    {
        if (Manager == null)
        {
            Manager = this;
        }

        MovementLock = false;
        InputActionManager.Manager.Inputs._1PInput.Move.started += PerformP1KeyInput;
        InputActionManager.Manager.Inputs._1PInput.Move.canceled += CancelP1KeyInput;
        InputActionManager.Manager.Inputs._1PInput.Jump.performed += JumpP1KeyInput;
        InputActionManager.Manager.Inputs._1PInput.Guard.performed += GuardP1KeyInput;
        InputActionManager.Manager.Inputs._1PInput.Guard.canceled += GuardP1KeyInputCancel;

        InputActionManager.Manager.Inputs._2PInput.Move.started += PerformP2KeyInput;
        InputActionManager.Manager.Inputs._2PInput.Move.canceled += CancelP2KeyInput;
        InputActionManager.Manager.Inputs._2PInput.Jump.performed += JumpP2KeyInput;
        InputActionManager.Manager.Inputs._2PInput.Guard.performed += GuardP2KeyInput;
        InputActionManager.Manager.Inputs._2PInput.Guard.canceled += GuardP2KeyInputCancel;
    }

    void Update()
    {
        GameManager.Manager.P1.Move(p1MoveDirection.x);
        GameManager.Manager.P2.Move(p2MoveDirection.x);
    }

    private void PerformP1KeyInput(InputAction.CallbackContext ctx)
    {
        GameManager.Manager.P1.GetComponentInChildren<CharacterAnimatorHandler>().StartWalkAnimation();

        p1MoveDirection = ctx.ReadValue<Vector2>();
    }

    private void PerformP2KeyInput(InputAction.CallbackContext ctx)
    {
        GameManager.Manager.P2.GetComponentInChildren<CharacterAnimatorHandler>().StartWalkAnimation();

        p2MoveDirection = ctx.ReadValue<Vector2>();
    }


    private void CancelP1KeyInput(InputAction.CallbackContext ctx)
    {
        p1MoveDirection = Vector2.zero;
        GameManager.Manager.P1.GetComponentInChildren<CharacterAnimatorHandler>().EndWalkAnimation();
    }

    private void CancelP2KeyInput(InputAction.CallbackContext ctx)
    {
        p2MoveDirection = Vector2.zero;
        GameManager.Manager.P2.GetComponentInChildren<CharacterAnimatorHandler>().EndWalkAnimation();
    }

    private void JumpP1KeyInput(InputAction.CallbackContext ctx)
    {
        GameManager.Manager.P1.Jump();
    }

    private void JumpP2KeyInput(InputAction.CallbackContext ctx)
    {
        GameManager.Manager.P2.Jump();
    }

    private void GuardP1KeyInput(InputAction.CallbackContext ctx)
    {
        Debug.Log("StartGuard");
        GameManager.Manager.P1.SetGuard();
    }

    private void GuardP2KeyInput(InputAction.CallbackContext ctx)
    {
        GameManager.Manager.P2.SetGuard();
    }

    private void GuardP1KeyInputCancel(InputAction.CallbackContext ctx)
    {
        Debug.Log("StopGuard");
        GameManager.Manager.P1.StopGuard();
    }

    private void GuardP2KeyInputCancel(InputAction.CallbackContext ctx)
    {
        GameManager.Manager.P2.StopGuard();
    }
}