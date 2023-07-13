using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    public abstract class ComplexCommandSO : CommandSO{
        [SerializeField] protected CommandSO[] commands;
    }
}