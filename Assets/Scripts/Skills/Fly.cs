using System.Collections;
using Characters.AnimationHandler;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Skill.Naktis
{
    public class Fly : MonoBehaviour, ICharacterSkill
    {
        public const string Name = "Fly";
        public bool HasHit { get; set; }
        public int Damage { get; set; } = 10;

        private bool isJumping;
        private NaktisAnimationHandler naktisAnimationHandler;
        private Coroutine flyCoroutine;

        public void SetCoff()
        {
        }

        public void Run()
        {
            Debug.Log("Start Fly");
            naktisAnimationHandler = transform.parent.GetComponent<NaktisAnimationHandler>();

            if (!flyCoroutine.IsUnityNull())
            {
                Debug.Log("Stop fly");
                naktisAnimationHandler.EndFlyAnimation();
                GetComponentInParent<Rigidbody2D>().gravityScale = ConstController.Manager.GravityScale;

                StopCoroutine(flyCoroutine);
                flyCoroutine = null;
            }
            else
            {
                flyCoroutine = StartCoroutine(NaktisFly());
                isJumping = true;
                GetComponentInParent<Player>().IsJumping = true;
            }

            Debug.Log("NaktisS1");
        }

        public void Hit()
        {
        }

        private IEnumerator NaktisFly()
        {
            //ConstController.Manager.GravityScale = GetComponentInChildren<Rigidbody2D>().gravityScale;
            Debug.Log("Fly Coroutin");
            naktisAnimationHandler.StartFlyAnimation();
            Rigidbody2D body = GetComponentInParent<Rigidbody2D>();
            body.gravityScale = 0;
            body.linearVelocityY = 0;
            body.AddForce(Vector2.up * ConstController.Manager.JumpForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(ConstController.Manager.WaitTime);
            body.AddForce(Vector2.down * ConstController.Manager.JumpForce, ForceMode2D.Impulse);

            float elapsedTime = 0;
            while (elapsedTime < ConstController.Manager.DurationTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            GetComponentInParent<Rigidbody2D>().gravityScale = ConstController.Manager.GravityScale;
            naktisAnimationHandler.EndFlyAnimation();
            flyCoroutine = null;
        }
    }
}