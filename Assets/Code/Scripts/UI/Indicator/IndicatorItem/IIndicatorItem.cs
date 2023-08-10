namespace Project.UI.Indicator{
    public interface IIndicatorItem{
        void SwitchToUnselectedUI();
        void SwitchToSelectedUI();
    }

    public interface IClickableIndicatorItem : IIndicatorItem{
        event System.Action OnClick;
    }

    public interface IProgressIndicatorItem : IIndicatorItem{
        void SwitchToUnReachableUI();
        void SwitchToCompletedUI();
    }

    public interface ICompletationIndicatorItem : IIndicatorItem{
        void SwitchToInCompletedUI();
        void SwitchToCompletedUI();
    }

    public interface ITrueAnswerIndicatorItem : IIndicatorItem{
        void SwitchToTrueOrFalseAnswerUI(bool isTrue);
    }
}