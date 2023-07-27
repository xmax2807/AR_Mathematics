using System.Threading.Tasks;

namespace Project.MiniGames.UI{
    public interface IAnimatableTaskUI{
        Task ShowAsync();
        Task HideAsync();

        event System.Action OnTaskShowed;
        event System.Action OnTaskHidden;
    }
}