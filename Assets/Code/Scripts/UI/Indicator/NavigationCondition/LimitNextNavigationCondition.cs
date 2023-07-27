namespace Project.UI.Indicator{
    public class LimitNextNavigationCondition : ComplexNavigationCondition
    {
        private bool[] m_listOfMovable;
        //private int currentIndex;
        public LimitNextNavigationCondition(INavigationCondition wrappee, int count) : base(wrappee, count)
        {
            InitMovableList(count);
        }

        public LimitNextNavigationCondition(int count) : base (new BasicNavigationCondition(count), count){
            InitMovableList(count);
        }

        private void InitMovableList(int count){
            m_listOfMovable = new bool[count];
            m_listOfMovable[0] = true;
            for(int i = 1; i < count; ++i){
                m_listOfMovable[i] = false; 
            }
        }

        public override bool CanMoveTo(int currentIndex, int destinationIndex)
        {
            
            bool result = m_wrappee.CanMoveTo(currentIndex, destinationIndex);

            if(result == false){
                return false;
            }

            return m_listOfMovable[destinationIndex];
        }

        public void UnlockNext(int index){
            if (index + 1 >= m_count)
            {
                return;
            }
            m_listOfMovable[index + 1] = true;
        }
    }
}