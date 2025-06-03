﻿using UnityEngine;
using System.Collections.Generic;
using System.Data;
using DataTable.FrameRanges;
using DataTable.DataSet;
using Manager;
using UnityEditor.Searcher;
using DataSet = DataTable.DataSet.DataSet;

namespace Handler
{
    public abstract class CharacterAnimatorHandler : MonoBehaviour
    {
        protected int baseLayerIndex, punchLayerIndex;

        protected Animator Animator;
        protected Transform PlayerTransform;

        private bool punchFlag;
        private bool additionalPunch;
        private Dictionary<string, bool> animationFlag;
        protected bool motionFlag = false;

        protected int CurrentLayerIndex;
        protected FrameRangesDictionary dictionary;

        protected abstract void FlagInitialize();

        public string State { get; set; }
        public int FrameIndex { get; set; }

        public GameObject Center { get; private set; }

        protected virtual void Awake()
        {
            CurrentLayerIndex = baseLayerIndex;
            Center = transform.Find("Center").gameObject;

            Animator = GetComponent<Animator>();
            PlayerTransform = transform;

            animationFlag = new Dictionary<string, bool>();
            animationFlag.Add("Punch", false);
            animationFlag.Add("Punch2", false);

            punchFlag = false;

            baseLayerIndex = Animator.GetLayerIndex("BaseLayer");
            punchLayerIndex = Animator.GetLayerIndex("Punch");
        }

        protected virtual void FixedUpdate()
        {
            CalFrameNumber();
        }

        public void StartPunchAnimation()
        {
            if (motionFlag)
            {
            }
            else
            {
                if (punchFlag && !additionalPunch)
                {
                    additionalPunch = true;
                }

                if (!punchFlag)
                {
                    LockMovement();
                    ChangeLayer(punchLayerIndex);
                    Animator.SetBool("PunchExit", false);
                    Animator.Play("Atk_Punch", CurrentLayerIndex, 0);
                    punchFlag = true;
                }
            }
        }

        public void StartJumpPunchAnimation()
        {
            if (!motionFlag)
            {
                Animator.Play("Jumping_Attack", CurrentLayerIndex, 0);
                motionFlag = true;
            }
        }

        public void AdditionalPunchAnimation()
        {
            if (additionalPunch)
            {
                Animator.SetTrigger("AdditionalPunch");
                LockMovement();
            }
            else
            {
                punchFlag = false;
            }
        }

        public void EndPunchAnimation()
        {
            if (!additionalPunch && !motionFlag)
            {
                PunchFlagInitialize();
                ChangeLayer(baseLayerIndex);
                Animator.SetBool("PunchExit", true);
                UnLockMovement();
            }
        }


        public void EndJumpPunchAnimation()
        {
            Debug.Log("EndJumpPunchAnimation");
            motionFlag = false;
        }

        public void EndKickAnimation()
        {
            if (!motionFlag)
            {
                PunchFlagInitialize();
                ChangeLayer(baseLayerIndex);
                Animator.SetBool("PunchExit", true);
                UnLockMovement();
            }
        }

        protected void PunchFlagInitialize()
        {
            punchFlag = false;
            additionalPunch = false;
        }

        public void StartHitAnimation()
        {
            LockMovement();
            ChangeLayer(baseLayerIndex);
            Animator.SetBool("Hit", true);
            motionFlag = true;
            FlagInitialize();
        }

        public void StartGuardAnimation()
        {
            LockMovement();
            Animator.SetBool("IsGuard", true);
        }

        public void EndGuardAnimation()
        {
            Animator.SetBool("IsGuard", false);
            UnLockMovement();
        }

        public void EndHitAnimation()
        {
            UnLockMovement();
            motionFlag = false;
            Animator.SetBool("Hit", false);
        }

        public void StartWalkAnimation()
        {
            Animator.SetBool("IsMove", true);
        }

        public void EndWalkAnimation()
        {
            Animator.SetBool("IsMove", false);
        }

        public void StartJumpAnimation()
        {
            Animator.SetTrigger("IsJump");
        }

        public void EndJumpAnimation()
        {
            Animator.Play("Jumping_Down", baseLayerIndex, 0);
            motionFlag = false;
        }

        protected void ChangeLayer(int targetLayerIndex)
        {
            Animator.SetLayerWeight(CurrentLayerIndex, 0);
            Animator.SetLayerWeight(targetLayerIndex, 1);
            CurrentLayerIndex = targetLayerIndex;
        }

        private void CalFrameNumber()
        {
            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(CurrentLayerIndex);
            if (!stateInfo.IsName("Empty"))
            {
                float normalizedTime = stateInfo.normalizedTime % 1f;

                AnimatorClipInfo[] clipInfo = Animator.GetCurrentAnimatorClipInfo(CurrentLayerIndex);
                AnimationClip clip = clipInfo[0].clip;
                int totalFrames = Mathf.RoundToInt(clip.length * clip.frameRate);
                int currentFrame = Mathf.FloorToInt(normalizedTime * totalFrames);

                State = clip.name;
                List<FrameRange> frameRanges =
                    dictionary.FrameRanges[State];
                for (int i = 0; i < frameRanges.Count; i++)
                {
                    if (currentFrame >= frameRanges[i].start && currentFrame <= frameRanges[i].end)
                    {
                        FrameIndex = i;
                        break;
                    }
                }

                HitBoxManager p1Manager = GameManager.Manager.P1.Player.GetComponentInParent<HitBoxManager>();
                HitBoxManager p2Manager = GameManager.Manager.P2.Player.GetComponentInParent<HitBoxManager>();

                if (gameObject.transform.parent.name.Equals("1P"))
                {
                    p1Manager.SetCurrentState(State, FrameIndex);
                    p2Manager.SetOpponentState(State, FrameIndex);
                }

                if (gameObject.transform.parent.name.Equals("2P"))
                {
                    p1Manager.SetOpponentState(State, FrameIndex);
                    p2Manager.SetCurrentState(State, FrameIndex);
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (CharacterAllStatement statement in GetComponentInParent<Player>().DataSet.RawData)
            {
                if (statement.Statement.Equals(State))
                {
                    foreach (var box in statement.FrameData[FrameIndex].HurtBoxes)
                    {
                        Vector2 playerCenter = Vector2.zero;
                        Vector2 center = Vector2.zero;
                        Vector2 size = Vector2.zero;
                        if (box.PartName.Equals("HitBox"))
                        {
                            Gizmos.color = Color.red;
                            playerCenter = PlayerTransform.position;
                            center = playerCenter + DataSet.FloatArrayToVector2(box.OffSet);
                            size = NaktisFrameDataSet.FloatArrayToVector2(box.Size);
                        }
                        else
                        {
                            playerCenter = PlayerTransform.position;
                            center = playerCenter + NaktisFrameDataSet.FloatArrayToVector2(box.OffSet);
                            size = NaktisFrameDataSet.FloatArrayToVector2(box.Size);
                            Gizmos.color = Color.green;
                        }

                        Gizmos.DrawWireCube(center, size);
                    }
                }
            }
        }

        protected void LockMovement()
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        protected void UnLockMovement()
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}