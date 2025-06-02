using System;
using System.Collections.Generic;
using Characters.Skill;
using Characters.Skill.Naktis;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class MethodGroup : MonoBehaviour
    {
        public GameObject Player { get; private set; }
        public Player playerComponent { get; private set; }
        
        public void Initialize()
        {
            Player = gameObject.transform.GetChild(0).gameObject;
            playerComponent = GetComponentInChildren<Player>();
            playerComponent.Initialize();
        }

        public void Move(float moveDirection)
        {
            Rigidbody2D body = Player.transform.GetComponent<Rigidbody2D>();
            body.linearVelocityX = moveDirection * ConstController.Manager.MoveSpeed;
        }
        
        public void Jump()
        {
            if (!playerComponent.IsJumping)
            {
                playerComponent.IsJumping = true;
                Rigidbody2D body = gameObject.transform.GetComponentInChildren<Rigidbody2D>();
                body.AddForce(Vector2.up * ConstController.Manager.JumpForce, ForceMode2D.Impulse);
            }
        }

        public void Punch()
        {
            playerComponent.GetComponentInChildren<Punch>().Run();
        }

        public void UseHasegi()
        {
            playerComponent.GetComponentInChildren<Hasegi>().Run();
        }

        public void UseScratch()
        {
            playerComponent.GetComponentInChildren<Scratch>().Run();
        }

        public void UseUpperWing()
        {
            playerComponent.GetComponentInChildren<UpperWing>().Run();
        }

        public void UseFly()
        {
            playerComponent.GetComponentInChildren<Fly>().Run();
        }


        public Action GetSkillAction(string skillName)
        {
            switch (skillName)
            {
                case Hasegi.Name:
                    return UseHasegi;
                case Scratch.Name:
                    return UseScratch;
                case UpperWing.Name:
                    return UseUpperWing;
                case Fly.Name:
                    return UseFly;
            }

            return null;
        }
    }
}