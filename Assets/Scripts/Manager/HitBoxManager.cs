using System.Collections.Generic;
using DataTable.DataSet;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class HitBoxManager : MonoBehaviour
    {
        [SerializeField] private Player currentPlayer;
        [SerializeField] private Player opponentPlayer;
        private string currentState;
        private string opponentCurrentState;

        private FrameNumberDictionary currentFrameData;
        private FrameNumberDictionary opponentCurrentFrameData;

        private int currentFrameIndex, opponentFrameIndex;

        public void SetCurrentState(string state, int frameIndex)
        {
            currentState = state;
            currentFrameData = currentPlayer.DataSet.Statements[state];
            currentFrameIndex = frameIndex;
        }

        public void SetOpponentState(string state, int frameIndex)
        {
            opponentCurrentState = state;
            opponentCurrentFrameData = opponentPlayer.DataSet.Statements[state];
            opponentFrameIndex = frameIndex;
        }

        void FixedUpdate()
        {
            if (currentFrameData != null && opponentCurrentFrameData != null)
            {
                var playerFrame = currentFrameData.Dictionary[currentFrameIndex + 1];
                var opponentFrame = opponentCurrentFrameData.Dictionary[opponentFrameIndex + 1];

                foreach (var playerBox in playerFrame.HurtBoxes)
                {
                    if (playerBox.PartName.Equals("HitBox"))
                    {
                        Vector2 playerCenter =
                            (Vector2)currentPlayer.transform.GetChild(0).GetChild(0).position +
                            DataSet.FloatArrayToVector2(playerBox.OffSet);
                        Vector2 playerSize = DataSet.FloatArrayToVector2(playerBox.Size);
                        Rect playerHitRect = new Rect(playerCenter - playerSize / 2f, playerSize);

                        foreach (var opponentBox in opponentFrame.HurtBoxes)
                        {
                            Vector2 opponentCenter =
                                (Vector2)opponentPlayer.transform.GetChild(0).GetChild(0)
                                    .position +
                                DataSet.FloatArrayToVector2(opponentBox.OffSet);
                            Vector2 opponentSize = DataSet.FloatArrayToVector2(opponentBox.Size);
                            Rect opponentHurtRect = new Rect(opponentCenter - opponentSize / 2f, opponentSize);
                            ICharacterSkill skill = GetHitSkill();
                            if (!skill.HasHit && playerHitRect.Overlaps(opponentHurtRect))
                            {
                                Debug.Log("Hit Detected!");
                                skill.HasHit = true;
                                opponentPlayer.Hit(skill.Damage);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private ICharacterSkill GetHitSkill()
        {
            return currentPlayer.PlayerSkills[currentState];
        }
    }
}