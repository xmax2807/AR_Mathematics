namespace Project.UI.Indicator{
    public class BasicNavigationCondition : INavigationCondition
    {
        private int m_count;
        public BasicNavigationCondition(int count){
            m_count = count;
        }
        public bool CanMoveTo(int currentIndex, int destinationIndex)
        {
            return  destinationIndex != currentIndex && destinationIndex >= 0 && destinationIndex < m_count;
        }
    }
}