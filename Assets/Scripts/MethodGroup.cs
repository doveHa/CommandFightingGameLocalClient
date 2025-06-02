using Characters.Skill;
using Characters.Skill.Naktis;
using UnityEngine;

namespace DefaultNamespace
{
    public class MethodGroup
    {
        private GameObject Player;
        private Player playerComponent;

        public MethodGroup(GameObject player)
        {
            Player = player;
            playerComponent = player.GetComponent<Player>();
        }

        public void Move(float moveDirection)
        {
            Movement.MoveCharacter(Player, moveDirection);
        }

        public void Jump()
        {
            Movement.JumpCharacter(Player);
        }

        public void Punch()
        {
            playerComponent.GetComponent<Punch>().Run();
        }

        public void UseHasegi()
        {
            playerComponent.GetComponent<Hasegi>().Run();
        }

        public void UseScratch()
        {
            playerComponent.GetComponent<Scratch>().Run();
        }

        public void UseUpperWing()
        {
            playerComponent.GetComponent<UpperWing>().Run();
        }

        public void UseFly()
        {
            playerComponent.GetComponent<Fly>().Run();
        }
    }
}