using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Vector2 moveDirection;

    void Start()
    {
        InputActionManager.Manager.Inputs.Movement.Move.started += PerformKeyInput;
        InputActionManager.Manager.Inputs.Movement.Move.canceled += CancelKeyInput;
        InputActionManager.Manager.Inputs.Movement.Jump.performed += JumpKeyInput;
    }

    void Update()
    {
        MoveCharacter(VarManager.Manager.PlayerGameObject.transform.GetChild(0).gameObject, moveDirection.x);
    }

    private void PerformKeyInput(InputAction.CallbackContext ctx)
    {
        GetComponentInChildren<Animator>().SetBool("IsMove", true);

        moveDirection = ctx.ReadValue<Vector2>();
        if ((moveDirection.x > 0 && !GameManager.Manager.IsPlayerLeft)
            || (moveDirection.x < 0 && GameManager.Manager.IsPlayerLeft))
        {
            VarManager.Manager.Player.SetGuard(true);
        }
        else
        {
            VarManager.Manager.Player.SetGuard(false);
        }
    }

    private void CancelKeyInput(InputAction.CallbackContext ctx)
    {
        moveDirection = Vector2.zero;
        VarManager.Manager.Player.SetGuard(false);
        GetComponentInChildren<Animator>().SetBool("IsMove", false);
    }

    private void JumpKeyInput(InputAction.CallbackContext ctx)
    {
        JumpCharacter(VarManager.Manager.PlayerGameObject.transform.GetChild(0).gameObject);
    }

    public void MoveCharacter(GameObject gameObject, float moveDirection)
    {
        Rigidbody2D body = gameObject.transform.GetComponent<Rigidbody2D>();
        body.linearVelocityX = moveDirection * ConstController.Manager.MoveSpeed;
    }

    public void JumpCharacter(GameObject gameObject)
    {
        Player player = gameObject.transform.GetComponentInParent<Player>();

        if (!player.IsJumping)
        {
            player.IsJumping = true;
            Rigidbody2D body = gameObject.transform.GetComponent<Rigidbody2D>();
            body.AddForce(Vector2.up * ConstController.Manager.JumpForce, ForceMode2D.Impulse);
        }
    }
}