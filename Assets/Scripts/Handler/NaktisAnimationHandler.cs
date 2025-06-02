﻿using System.Collections.Generic;
using DataTable.FrameRanges;
using Handler;
using UnityEngine;

namespace Characters.AnimationHandler
{
    public class NaktisAnimationHandler : CharacterAnimatorHandler
    {
        private Dictionary<string, bool> animationFlag;

        public bool HasegiMotion { get; set; }
        public bool FirstScratch { get; set; }
        public bool SecondScratch { get; set; }


        protected override void Awake()
        {
            base.Awake();
            dictionary = new NaktisFrameRangesDictionary();
            animationFlag = new Dictionary<string, bool>();
            animationFlag.Add("Hasegi", false);
            animationFlag.Add("Scratch", false);
            animationFlag.Add("UpperWing", false);

            HasegiMotion = false;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void StartHasegiAnimation()
        {
            if (!animationFlag["Hasegi"] && !motionFlag)
            {
                motionFlag = true;
                animationFlag["Hasegi"] = true;
                Animator.SetTrigger("Hasegi");
            }
        }

        public void HasegiMotionTrue()
        {
            HasegiMotion = true;
        }

        public void FlagHasegiFalse()
        {
            animationFlag["Hasegi"] = false;
            motionFlag = false;
            HasegiMotion = false;
        }

        public void StartScratchAnimation()
        {
            if (!animationFlag["Scratch"] && !motionFlag)
            {
                motionFlag = true;
                animationFlag["Scratch"] = true;
                Animator.SetTrigger("Scratch");
            }
        }

        public void FirstScratchOn()
        {
            FirstScratch = true;
        }

        public void SecondScratchOn()
        {
            SecondScratch = true;
        }

        public void FlagScratchFalse()
        {
            animationFlag["Scratch"] = false;
            motionFlag = false;
            FirstScratch = false;
            SecondScratch = false;
        }

        public void StartUpperWingAnimation()
        {
            if (!animationFlag["UpperWing"] && !motionFlag)
            {
                motionFlag = true;
                animationFlag["UpperWing"] = true;
                Animator.SetTrigger("UpperWing");
            }
        }

        public void FlagUpperWingFalse()
        {
            animationFlag["UpperWing"] = false;
            motionFlag = false;
            Debug.Log(motionFlag);
        }

        public void StartFlyAnimation()
        {
            Debug.Log("StartFlyAnimation");
            Animator.SetBool("IsFlying", true);
        }

        public void EndFlyAnimation()
        {
            Animator.SetBool("IsFlying", false);
            foreach (AnimatorControllerParameter parameter in Animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    Animator.ResetTrigger(parameter.name);
                    animationFlag[parameter.name] = false;
                }
            }

            motionFlag = false;
        }
    }
}