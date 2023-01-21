using System.Collections;
using Project.Utils.ExtensionMethods;
using UnityEngine;

namespace Project.MiniGames.FishingGame
{
    [RequireComponent(typeof(Animator))]
    public class BaseCharacter : MonoBehaviour
    {
        private Animator _animator;
        public Animator Animator => _animator;

        private void OnValidate()
        {
            this.EnsureComponent<Animator>(ref _animator);
        }
    }
}
