using DataTable.DataSet;
using Handler;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public Vector2 Position;
    private SpriteRenderer spriteRenderer;
    public CharacterAnimatorHandler Animator { get; set; }
    private DataSet leftSide, rightSide;
    public DataSet DataSet { get; set; }

    private bool isLeft;
    public bool isGuard;
    public bool IsAirborne;
    public bool IsJumping { get; set; }

    private int health = 100;

    public void Initialize()
    {
        isLeft = true;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponentInChildren<CharacterAnimatorHandler>();
    }

    public void Flip()
    {
        spriteRenderer.flipX = isLeft;
        isLeft = !isLeft;

        if (isLeft)
        {
            DataSet.SetLeftSide();
        }
        else
        { 
            DataSet.SetRightSide();
        }
    }

    public void SetGuard(bool isGuard)
    {
        this.isGuard = isGuard;
    }

    public void Airborne(int atk)
    {
        Rigidbody2D rb = GetComponentInChildren<Rigidbody2D>();
        if (isGuard)
        {
            Animator.StartGuardAnimation();
        }
        else if (IsAirborne)
        {
            rb.AddForce(Vector2.up * ConstController.Manager.ReAirbonneForceY, ForceMode2D.Impulse);
            Animator.ReAirborneHitAnimation();
        }
        else
        {
            IsAirborne = true;
            Animator.StartAirborneAnimation();

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * ConstController.Manager.AirborneForceY, ForceMode2D.Impulse);
            health -= atk;
        }
    }

    public void Hit(int atk)
    {
        if (isGuard)
        {
            Debug.Log("guard");
            Animator.StartGuardAnimation();
        }
        else if (IsAirborne)
        {
            Debug.Log("Re");
            GetComponentInChildren<Rigidbody2D>()
                .AddForce(Vector2.up * ConstController.Manager.ReAirbonneForceY, ForceMode2D.Impulse);
            Animator.ReAirborneHitAnimation();
        }
        else
        {
            Animator.StartHitAnimation();
            health -= atk;
            Debug.Log(health);
        }
    }

    public void SetDataSet(string charaterName)
    {
        switch (charaterName)
        {
            case "Naktis":
                DataSet = new NaktisFrameDataSet();
                DataSet.SetLeftSide();
                break;
            case "Kaegetsu":
                //DataSet = new KagetsuFrameDataSet();
                DataSet.SetLeftSide();
                break;
            case "Vargon":
                //DataSet = new VargonFrameDataSet();
                DataSet.SetLeftSide();
                break;
        }
    }
}