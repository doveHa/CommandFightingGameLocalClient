﻿using Handler;
using UnityEngine;

namespace Manager
{
    public class InputActionManager : MonoBehaviour
    {
        public static InputActionManager Manager { get; private set; }
        public ActionsInput Inputs { get; private set; }

        void Awake()
        {
            DontDestroyOnLoad(this);
            if (Manager == null)
            {
                Manager = this;
            }

            Inputs = new ActionsInput();
        }

        void OnEnable()
        {
            Inputs.Enable();
        }

        void OnDisable()
        {
            Inputs.Disable();
        }
    }
}