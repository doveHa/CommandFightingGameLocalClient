using System;
using System.Collections.Generic;
using Characters;
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
            foreach (KeyValuePair<string, List<string>> keyValuePair in GameManager.Manager.SkillCommand)
            {
                AddCombo(keyValuePair.Key, keyValuePair.Value);
            }
            InputActionManager.Manager.Inputs.Command.CommandInput.performed += OnInputPerformed;
        }

        public void AddCombo(string skillName, List<string> commands)
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

            currentNode.Commands = commands;
            currentNode.SkillName = skillName;
            currentNode.IsEndOfCombo = true;
        }

        public void ChangeCombo(string skillName, List<string> newCommands)
        {
            if (!RemoveCombo(_comboTireRoot, skillName))
            {
                Debug.LogWarning($"Combo with skill name '{skillName}' was not found to remove.");
            }

            AddCombo(skillName, newCommands);
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
                    if (nextNode.IsEndOfCombo)
                    {
                        InputActionManager.Manager.Inputs.Atk.Atk.Disable();
                        VarManager.Manager.PlayerSkills[nextNode.SkillName].Run();
                        //nextNode.SkillInfo.Action.Invoke(nextNode.SkillInfo);
                        comboExecuted = true;
                        InputActionManager.Manager.Inputs.Atk.Atk.Enable();

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