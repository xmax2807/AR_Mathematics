using System;
using TMPro;
using UnityEngine;

namespace Project.UI.Indicator{
    public class TextIndicatorItem : MonoBehaviour, IIndicatorItem
    {
        [SerializeField] private TextMeshProUGUI presenter;
        [SerializeField] private Color unselectedColor = Color.white;
        [SerializeField] private Color selectedColor = Color.white;

        public void ChangeText(string text) => presenter.text = text;
        public void SwitchToSelectedUI()
        {
            presenter.color = selectedColor;
        }

        public void SwitchToUnselectedUI()
        {
            presenter.color = unselectedColor;
        }

        internal void ChangeText(object value)
        {
            throw new NotImplementedException();
        }
    }
}