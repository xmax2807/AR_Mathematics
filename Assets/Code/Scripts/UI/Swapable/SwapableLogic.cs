namespace Project.UI{
    public interface ISwapableLogic<T>{
        public T GetData();
    }
    public class SwapableLogic<T> : ISwapableLogic<T>
    {
        private T data;
        public SwapableLogic(T data){
            this.data = data;
        }
        public T GetData()
        {
            return data;
        }
    }
    public class SwapableLogicDefault<T> : ISwapableLogic<T>{
        private T data;
        private SwapableLogic<T> RealLogic;
        public System.Func<bool> OnCheckCondition;
        public SwapableLogicDefault(T data, SwapableLogic<T> realLogic, System.Func<bool> onCheckCondition){
            this.data = data; 
            RealLogic = realLogic;
            this.OnCheckCondition = onCheckCondition;
        }
        public T GetData(){
            bool isConditionMet = OnCheckCondition != null && OnCheckCondition.Invoke();
            if(isConditionMet){
                return RealLogic.GetData();
            }
            return data;
        }
    }
}