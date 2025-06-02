﻿using System.Collections;
using Characters.AnimationHandler;
using Manager;
using UnityEngine;

namespace Characters.Skill.Naktis
{
    public class Hasegi : MonoBehaviour, ICharacterSkill
    {
        public const string Name = "Hasegi";   

        public bool HasHit { get; set; }
                public int Damage { get; set; } = 10;


        private float speed = 10f;
        private NaktisAnimationHandler naktisAnimationHandler;

        public void SetCoff()
        {
        }

        public void Run()
        {
            HasHit = false;
            naktisAnimationHandler = transform.parent.GetComponent<NaktisAnimationHandler>();
            naktisAnimationHandler.StartHasegiAnimation();
            StartCoroutine(WaitHasegiMotion());
        }
        
        private IEnumerator WaitHasegiMotion()
        {
            yield return new WaitUntil(() => naktisAnimationHandler.ShootHasegi);

            CreateWind();
        }

        public void CreateWind()
        {
            Vector2 HasegiDirection = gameObject.transform.Find("HasegiStartTransform").position;
            GameObject hasegi = Instantiate(Resources.Load<GameObject>("Prefabs/Character/Naktis/Skill/Hasegi"),
                HasegiDirection, Quaternion.identity);
            Vector3 direction = (GameManager.Manager.OpponentCenter - GameManager.Manager.PlayerCenter).normalized;
            Rigidbody2D rigidbody = hasegi.GetComponent<Rigidbody2D>();

            rigidbody.linearVelocity = direction * speed;

            naktisAnimationHandler.ShootHasegi = false;
        }
    }
}