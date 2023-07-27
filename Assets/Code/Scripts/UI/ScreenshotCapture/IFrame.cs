using UnityEngine;

namespace Project.UI.Screenshot{
    public interface IFrame {
        Vector2 GetSize();
        Vector2 GetLocalPosition();
        Vector2 GetPosition();
    }
}