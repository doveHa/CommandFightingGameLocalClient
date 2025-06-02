using System.Collections.Generic;
using Characters.Skill;
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

        public Dictionary<string, List<string>> SkillCommand { get; set; }

        void Awake()
        {
            DontDestroyOnLoad(this);
            if (Manager == null)
            {
                Manager = this;
            }

            SkillCommand = new Dictionary<string, List<string>>();
            CommandInitialize();
        }

        void Start()
        {
            Initialize();

            wasPlayerLeft = true;
            VarManager.Manager.Opponent.Flip();
        }

        void Update()
        {
            IsPlayerLeft = CalculatePlayerIsLeft();

            if (wasPlayerLeft != IsPlayerLeft)
            {
                VarManager.Manager.Player.Flip();
                VarManager.Manager.Opponent.Flip();
                wasPlayerLeft = IsPlayerLeft;
            }
        }

        private bool CalculatePlayerIsLeft()
        {
            float playerX = VarManager.Manager.PlayerGameObject.transform.position.x;
            float opponentX = VarManager.Manager.OpponentGameObject.transform.position.x;
            return playerX < opponentX;
        }

        private void Initialize()
        {
            VarManager.Manager.PlayerCharacterName = "Naktis";
            VarManager.Manager.OpponentCharacterName = "Naktis";

            VarManager.Manager.PlayerGameObject = GameObject.Find("Player");
            VarManager.Manager.OpponentGameObject = GameObject.Find("Opponent");

            Instantiate(
                Resources.Load<GameObject>("Prefab/Naktis"),
                VarManager.Manager.PlayerGameObject.transform).tag = "Player";

            Instantiate(
                Resources.Load<GameObject>("Prefab/Naktis"),
                VarManager.Manager.OpponentGameObject.transform).tag = "Opponent";
            Destroy(VarManager.Manager.OpponentGameObject.GetComponentInChildren<Punch>().gameObject);
            VarManager.Manager.OpponentGameObject.GetComponentInChildren<Rigidbody2D>().constraints =
                RigidbodyConstraints2D.FreezeAll;

            VarManager.Manager.PlayerOpponentInitialize();
        }

        private void CommandInitialize()
        {
            List<string> fly = new List<string>();
            fly.Add("UPARROW");
            fly.Add("Z");
            SkillCommand.Add("Fly", fly);

            List<string> scratch = new List<string>();
            scratch.Add("DOWNARROW");
            scratch.Add("DOWNARROW");
            scratch.Add("Z");
            SkillCommand.Add("Scratch", scratch);

            List<string> hasegi = new List<string>();
            hasegi.Add("UPARROW");
            hasegi.Add("RIGHTARROW");
            hasegi.Add("Z");
            SkillCommand.Add("Hasegi", hasegi);

            List<string> upperWing = new List<string>();
            upperWing.Add("LEFTARROW");
            upperWing.Add("RIGHTARROW");
            upperWing.Add("Z");
            SkillCommand.Add("UpperWing", upperWing);
        }
    }
}