using Handler;
using Manager;
using UnityEngine;

namespace Characters.Skill
{
    public class Punch : MonoBehaviour, ICharacterSkill
    {
        public const string Name = "Punch";

        public bool HasHit { get; set; }
        public int Damage { get; set; } = 10;

        void Start()
        {
            InputActionManager.Manager.Inputs._1PInput.BasicAtk.started += (ctx => { GameManager.Manager.P1.Punch(); });
            InputActionManager.Manager.Inputs._2PInput.BasicAtk.started += (ctx => { GameManager.Manager.P2.Punch(); });
        }

        public void Run()
        {
            HasHit = false;
            CharacterAnimatorHandler animator = GetComponentInParent<CharacterAnimatorHandler>();
            animator.StartPunchAnimation();
        }
    }
}