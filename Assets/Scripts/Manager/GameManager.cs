using System.Collections.Generic;
using Characters.Skill;
using Characters.Skill.Naktis;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public Vector2 PlayerCenter { get; set; }
        public Vector2 OpponentCenter { get; set; }

        private bool wasPlayerLeft;
        public bool IsPlayerLeft { get; private set; }

        public static GameManager Manager { get; private set; }

        public MethodGroup P1, P2;
        public Dictionary<string, List<string>> P1Skills { get; set; }
        public Dictionary<string, List<string>> P2Skills { get; set; }


        void Awake()
        {
            DontDestroyOnLoad(this);
            if (Manager == null)
            {
                Manager = this;
            }

            P1 = GameObject.Find("1P").GetComponent<MethodGroup>();
            P2 = GameObject.Find("2P").GetComponent<MethodGroup>();
            P1Skills = new Dictionary<string, List<string>>();
            P2Skills = new Dictionary<string, List<string>>();

            CommandInitialize();
        }

        void Start()
        {
            Initialize();

            wasPlayerLeft = true;
            P2.playerComponent.Flip();
        }

        void Update()
        {
            IsPlayerLeft = CalculatePlayerIsLeft();

            if (wasPlayerLeft != IsPlayerLeft)
            {
                P1.playerComponent.Flip();
                P2.playerComponent.Flip();

                wasPlayerLeft = IsPlayerLeft;
            }
        }

        private bool CalculatePlayerIsLeft()
        {
            float playerX = P1.Player.transform.position.x;
            float opponentX = P2.Player.transform.position.x;
            return playerX < opponentX;
        }

        private void Initialize()
        {
            Instantiate(
                Resources.Load<GameObject>("Prefab/Naktis"),
                P1.transform).tag = "Player";
            P1.Initialize();
            Instantiate(
                Resources.Load<GameObject>("Prefab/Naktis"),
                P2.transform).tag = "Opponent";
            P2.Initialize();
        }


        private void CommandInitialize()
        {
            List<string> fly1 = new List<string>();
            fly1.Add("W");
            fly1.Add("B");
            P1Skills.Add(Fly.Name, fly1);

            List<string> fly2 = new List<string>();
            fly2.Add("UPARROW");
            fly2.Add("NUMPAD1");
            P2Skills.Add(Fly.Name, fly2);

            List<string> scratch1 = new List<string>();
            scratch1.Add("S");
            scratch1.Add("S");
            scratch1.Add("B");
            P1Skills.Add(Scratch.Name, scratch1);

            List<string> scratch2 = new List<string>();
            scratch2.Add("DOWNARROW");
            scratch2.Add("DOWNARROW");
            scratch2.Add("NUMPAD1");
            P2Skills.Add(Scratch.Name, scratch2);

            List<string> hasegi1 = new List<string>();
            hasegi1.Add("W");
            hasegi1.Add("D");
            hasegi1.Add("B");
            P1Skills.Add(Hasegi.Name, hasegi1);

            List<string> hasegi2 = new List<string>();
            hasegi2.Add("UPARROW");
            hasegi2.Add("RIGHTARROW");
            hasegi2.Add("NUMPAD1");
            P2Skills.Add(Hasegi.Name, hasegi2);

            List<string> upperWing1 = new List<string>();
            upperWing1.Add("A");
            upperWing1.Add("D");
            upperWing1.Add("B");
            P1Skills.Add(UpperWing.Name, upperWing1);

            List<string> upperWing2 = new List<string>();
            upperWing2.Add("LEFTARROW");
            upperWing2.Add("RIGHTARROW");
            upperWing2.Add("NUMPAD1");
            P2Skills.Add(UpperWing.Name, upperWing2);
        }
    }
}