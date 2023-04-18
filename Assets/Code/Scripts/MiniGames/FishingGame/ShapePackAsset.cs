using Project.QuizSystem;
using UnityEngine;

namespace Project.MiniGames.FishingGame
{
    [CreateAssetMenu(menuName = "STO/FishingGame/ShapeAsset", fileName = "ShapeAsset")]
    public class ShapePackAsset : ScriptableObject
    {
        [System.Serializable]
        public struct ShapePack
        {
            public Shape.ShapeType Type;
            public Sprite Icon;
        }
        public ShapePack[] Packs;
    }
}