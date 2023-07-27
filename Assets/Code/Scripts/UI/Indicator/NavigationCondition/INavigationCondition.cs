namespace Project.UI.Indicator{
    public interface INavigationCondition {
        bool CanMoveTo(int currentIndex, int destinationIndex);
    }
}