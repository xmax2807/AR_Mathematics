using System.Threading;
using System.Threading.Tasks;
using Project.UI.Event.Popup;

namespace Project.UI.Panel{
    public class AutoNotificationPanelView : NotificationPanelView
    {
        protected override void SetAdditionData(PopupData data)
        {
        }
        public void Close(){
            Ok();
        }
    }
}