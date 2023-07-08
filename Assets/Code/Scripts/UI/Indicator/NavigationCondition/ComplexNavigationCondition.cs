namespace Project.UI.Indicator{
    public abstract class ComplexNavigationCondition : INavigationCondition
    {
        protected int m_count;
        protected INavigationCondition m_wrappee;
        public ComplexNavigationCondition(INavigationCondition wrappee, int count){
            this.m_count = count;
            this.m_wrappee = wrappee;
        }
        public abstract bool CanMoveTo(int currentIndex, int destinationIndex);
    }
}