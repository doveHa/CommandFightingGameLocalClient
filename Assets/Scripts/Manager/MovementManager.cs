using DefaultNamespace;
using Handler;
using Manager;
using RestSharp;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementManager : MonoBehaviour
{
    private Vector2 p1MoveDirection;
    private Vector2 p2MoveDirection;

    void Start()
    {
        InputActionManager.Manager.Inputs._1PInput.Move.started += PerformP1KeyInput;
        InputActionManager.Manager.Inputs._1PInput.Move.canceled += CancelP1KeyInput;
        InputActionManager.Manager.Inputs._1PInput.Jump.performed += JumpP1KeyInput;

        InputActionManager.Manager.Inputs._2PInput.Move.started += PerformP2KeyInput;
        InputActionManager.Manager.Inputs._2PInput.Move.canceled += CancelP2KeyInput;
        InputActionManager.Manager.Inputs._2PInput.Jump.performed += JumpP2KeyInput;
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
        if ((p1MoveDirection.x > 0 && !GameManager.Manager.IsPlayerLeft)
            || (p1MoveDirection.x < 0 && GameManager.Manager.IsPlayerLeft))
        {
            GameManager.Manager.P1.playerComponent.SetGuard(true);
        }
        else
        {
            GameManager.Manager.P1.playerComponent.SetGuard(true);
        }
    }

    private void PerformP2KeyInput(InputAction.CallbackContext ctx)
    {
        GameManager.Manager.P2.GetComponentInChildren<CharacterAnimatorHandler>().StartWalkAnimation();

        p2MoveDirection = ctx.ReadValue<Vector2>();
        if ((p2MoveDirection.x > 0 && !GameManager.Manager.IsPlayerLeft)
            || (p2MoveDirection.x < 0 && GameManager.Manager.IsPlayerLeft))
        {
            GameManager.Manager.P2.playerComponent.SetGuard(true);
        }
        else
        {
            GameManager.Manager.P2.playerComponent.SetGuard(true);
        }
    }


    private void CancelP1KeyInput(InputAction.CallbackContext ctx)
    {
        p1MoveDirection = Vector2.zero;
        GameManager.Manager.P1.playerComponent.SetGuard(false);
        GameManager.Manager.P1.GetComponentInChildren<CharacterAnimatorHandler>().EndWalkAnimation();
    }

    private void CancelP2KeyInput(InputAction.CallbackContext ctx)
    {
        p2MoveDirection = Vector2.zero;
        GameManager.Manager.P2.playerComponent.SetGuard(false);
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
}