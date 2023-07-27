using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Project.RewardSystem
{
    [CreateAssetMenu(fileName = "RewardPackSTO", menuName = "STO/Reward/RewardPackSTO", order = 1)]
    public class RewardPackSTO : ScriptableObject
    {
        [SerializeField] private RewardBadgeSTO artist;
        [SerializeField] private RewardBadgeSTO builder;
        [SerializeField] private RewardBadgeSTO compass;
        [SerializeField] private RewardBadgeSTO designer;
        [SerializeField] private RewardBadgeSTO fishing;
        [SerializeField] private RewardBadgeSTO godOfTime;
        [SerializeField] private RewardBadgeSTO mathGenius;
        [SerializeField] private RewardBadgeSTO ruler;
        public Sprite defaultSprite;
        private static Dictionary<string, RewardBadgeSTO> spriteDictionary;
        public int PackCount => spriteDictionary.Count;
        public string[] Keys => spriteDictionary.Keys.ToArray();
        void OnEnable()
        {
            if(spriteDictionary != null) return;
            spriteDictionary = new()// Static dictionary to map Ids to sprites
            {
                {"Compass", this.compass},
                {"Designer", this.designer},
                {"Artist", this.artist},
                {"Builder", this.builder},
                {"Fishing", this.fishing},
                {"God of Time", this.godOfTime},
                {"Math Genius", this.mathGenius},
                {"Ruler", this.ruler},
            };
        }

        public RewardBadgeSTO GetReward(string id)
        {
            return spriteDictionary[id];
        }
    }
}