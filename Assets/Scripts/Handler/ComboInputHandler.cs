using System;
using System.Collections.Generic;
using Characters;
using DefaultNamespace;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Handler
{
    public class ComboInputHandler : MonoBehaviour
    {
        public class ComboTireNode
        {
            public Dictionary<string, ComboTireNode> Children = new();
            public string SkillName;
            public MethodGroup Group;
            public List<string> Commands;
            public bool IsEndOfCombo;
        }

        private struct TimedComboNode
        {
            public ComboTireNode Node;
            public float ExpireTime;
        }

        private const float TimeLimit = 2_000f; // 밀리초
        private const int MaxQueueSize = 4;

        private readonly ComboTireNode _comboTireRoot = new();
        private readonly TimedComboNode[] _inputBuffer = new TimedComboNode[MaxQueueSize];
        private int _inputCount;

        private void Start()
        {
            foreach (KeyValuePair<string, List<string>> keyValuePair in GameManager.Manager.P1Skills)
            {
                AddCombo(keyValuePair.Key, GameManager.Manager.P1, keyValuePair.Value);
            }

            foreach (KeyValuePair<string, List<string>> keyValuePair in GameManager.Manager.P2Skills)
            {
                AddCombo(keyValuePair.Key, GameManager.Manager.P2, keyValuePair.Value);
            }

            InputActionManager.Manager.Inputs._1PInput.CommandInput.performed += OnInputPerformed;
            InputActionManager.Manager.Inputs._2PInput.CommandInput.performed += OnInputPerformed;
        }

        public void AddCombo(string skillName, MethodGroup group, List<string> commands)
        {
            List<string> keySequence = commands;
            ComboTireNode currentNode = _comboTireRoot;

            foreach (string key in keySequence)
            {
                if (currentNode.IsEndOfCombo) throw new UnReachableComboException();

                if (!currentNode.Children.TryGetValue(key, out ComboTireNode nextNode))
                {
                    nextNode = new ComboTireNode();
                    currentNode.Children.Add(key, nextNode);
                }

                currentNode = nextNode;
            }

            currentNode.SkillName = skillName;
            currentNode.Group = group;
            currentNode.Commands = commands;

            currentNode.IsEndOfCombo = true;
        }
        private bool RemoveCombo(ComboTireNode node, string skillName, int depth = 0)
        {
            // Leaf node case
            if (node.IsEndOfCombo && node.SkillName == skillName)
            {
                node.IsEndOfCombo = false;
                node.SkillName = null;
                node.Commands = null;
                return true;
            }

            List<string> keysToRemove = new List<string>();

            foreach (var kvp in node.Children)
            {
                ComboTireNode child = kvp.Value;

                if (RemoveCombo(child, skillName, depth + 1))
                {
                    // 만약 자식이 더 이상 경로를 가지지 않는다면 정리
                    if (!child.IsEndOfCombo && child.Children.Count == 0)
                    {
                        keysToRemove.Add(kvp.Key);
                    }
                }
            }

            foreach (string key in keysToRemove)
            {
                node.Children.Remove(key);
            }

            return keysToRemove.Count > 0;
        }

        public void OnInputPerformed(InputAction.CallbackContext ctx)
        {
            string context = ctx.control.name;
            Debug.Log(context.ToUpper());
            ProcessInput(context.ToUpper());
        }

        private void ProcessInput(string inputKey)
        {
            float currentTime = Time.time;
            float expirationTime = currentTime + (TimeLimit / 1000f);

            int processedCount = 0;
            bool comboExecuted = false;

            for (int i = 0; i < _inputCount; i++)
            {
                TimedComboNode timedNode = _inputBuffer[i];

                if (timedNode.Node.Children.TryGetValue(inputKey, out ComboTireNode nextNode))
                {
                    Debug.Log("key");
                    if (nextNode.IsEndOfCombo)
                    {
                        InputActionManager.Manager.Inputs._1PInput.BasicAtk.Disable();
                        InputActionManager.Manager.Inputs._2PInput.BasicAtk.Disable();

                        nextNode.Group.GetSkillAction(nextNode.SkillName).Invoke();
                        //nextNode.SkillInfo.Action.Invoke(nextNode.SkillInfo);
                        comboExecuted = true;
                        InputActionManager.Manager.Inputs._1PInput.BasicAtk.Enable();
                        InputActionManager.Manager.Inputs._2PInput.BasicAtk.Enable();

                        break;
                    }

                    _inputBuffer[processedCount++] = new TimedComboNode
                    {
                        Node = nextNode,
                        ExpireTime = expirationTime
                    };
                }
            }

            if (comboExecuted)
            {
                _inputCount = 0;
                return;
            }

            _inputCount = processedCount;

            if (_comboTireRoot.Children.TryGetValue(inputKey, out ComboTireNode rootNextNode))
            {
                if (rootNextNode.Children.Count > 0)
                {
                    if (_inputCount >= MaxQueueSize)
                    {
                        Array.Copy(_inputBuffer, 1, _inputBuffer, 0, MaxQueueSize - 1);
                        _inputCount--;
                    }

                    _inputBuffer[_inputCount++] = new TimedComboNode
                    {
                        Node = rootNextNode,
                        ExpireTime = expirationTime
                    };
                }
            }
        }


        private void Update()
        {
            float currentTime = Time.time;
            int writeIndex = 0;

            for (int i = 0; i < _inputCount; i++)
            {
                if (_inputBuffer[i].ExpireTime > currentTime)
                {
                    _inputBuffer[writeIndex++] = _inputBuffer[i];
                }
            }

            _inputCount = writeIndex;
        }

        public class UnReachableComboException : Exception
        {
            public UnReachableComboException() : base("Unreachable combo")
            {
            }
        }
    }
}