using UnityEngine;

namespace Project.UI.Panel{
    public class ProfilePanelController : BasePanelController
    {
        [SerializeField] TMPro.TextMeshProUGUI userIdText;
        [SerializeField] TMPro.TextMeshProUGUI emailText;
        public override PanelEnumType Type => PanelEnumType.Profile;

        public override bool CheckType(PanelViewData data)
        {
            return data.Type == this.Type;
        }

        public override void SetUI(PanelViewData Data)
        {
            Firebase.Auth.FirebaseUser currentUser = DatabaseManager.Auth.CurrentUser;
            userIdText.text = currentUser.UserId;
            emailText.text = currentUser.Email;
        }
    }
}