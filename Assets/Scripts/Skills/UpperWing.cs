using Characters.AnimationHandler;
using UnityEngine;
using Manager;

namespace Characters.Skill.Naktis
{
    public class UpperWing : MonoBehaviour, ICharacterSkill
    {
        public const string Name = "UpperWing";   

        public bool HasHit { get; set; }
        public int Damage { get; set; } = 10;

        private NaktisAnimationHandler naktisAnimationHandler;

        public void SetCoff()
        {
        }

        public void Run()
        {
            HasHit = false;
            naktisAnimationHandler = transform.parent.GetComponent<NaktisAnimationHandler>();
            naktisAnimationHandler.StartUpperWingAnimation();
        }
    }
}