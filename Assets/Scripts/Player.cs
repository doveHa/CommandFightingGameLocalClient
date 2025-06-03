using System.Collections.Generic;
using Characters.Skill;
using Characters.Skill.Naktis;
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
    public Dictionary<string, ICharacterSkill> PlayerSkills;

    public bool isLeft { get; private set; }
    public bool isGuard;
    public bool IsJumping { get; set; }
    
    private int health = 100;

    public void Initialize()
    {
        isLeft = true;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponentInChildren<CharacterAnimatorHandler>();

        SetDataSet("Naktis");
        PlayerSkills = new Dictionary<string, ICharacterSkill>();
        PlayerSkills.Add("Atk_Punch", GetComponentInChildren<Punch>());
        PlayerSkills.Add("Atk_Kick", GetComponentInChildren<Punch>());
        PlayerSkills.Add("Hasegi", GetComponentInChildren<Hasegi>());
        PlayerSkills.Add("Scratch", GetComponentInChildren<Scratch>());
        PlayerSkills.Add("UpperWing", GetComponentInChildren<UpperWing>());
        PlayerSkills.Add("Fly_Drop", GetComponentInChildren<Fly>());
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


    public void Hit(int atk)
    {
        if (isGuard)
        {
            Debug.Log("guard");
            Animator.StartGuardAnimation();
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