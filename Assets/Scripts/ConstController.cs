using UnityEngine;

public class ConstController : MonoBehaviour
{
    public static ConstController Manager { get; private set; }
    //public float NaktisFlyYPosition = 0.25f;

    [Tooltip("체공 지속 시간")] public float DurationTime = 4;

    [Tooltip("올라가는 속도")] public float JumpForce = 5f;

    [Tooltip("떨어지는 속도")] public float FallGravitySacle = 1f;
    
    [Tooltip("기본 중력")] public float GravityScale = 1f;

    [Tooltip("올라가는 시간")] public float WaitTime = 1;

    [Tooltip("이동 속도")] public float MoveSpeed = 1f;


    void Awake()
    {
        Manager = this;
        DontDestroyOnLoad(gameObject);
    }
}