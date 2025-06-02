using Handler;
using Manager;
using UnityEngine;

namespace Characters.Skill
{
    public class Punch : MonoBehaviour, ICharacterSkill
    {
        public bool HasHit { get; set; }

        void Start()
        {
            InputActionManager.Manager.Inputs.Atk.Atk.started += (ctx => { AddPunch(); });
        }

        private void AddPunch()
        {
            Debug.Log("Press Punch");
            VarManager.Manager.PlayerSkills["Atk_Punch"].Run();
        }
        public void Run()
        {
            HasHit = false;
            CharacterAnimatorHandler animator = GetComponentInParent<CharacterAnimatorHandler>();
            animator.StartPunchAnimation();
        }

        public void Hit()
        {
            HasHit = true;
            VarManager.Manager.Opponent.Hit(10);
        }
    }
}