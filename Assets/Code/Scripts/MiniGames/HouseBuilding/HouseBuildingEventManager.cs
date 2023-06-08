namespace Project.MiniGames.HouseBuilding{
    public class HouseBuildingEventManager : ARGameEventManager{
        #region ConstEventName
        public const string ReturnPrevBuildEventName = "ReturnPrevBuildEvent";
        public const string BlockTouchEventName = "BlockTouchEvent";
        public const string BlockPlacedEventName = "BlockPlacedEvent";
        public const string BuildCompleteEventName = "BuildCompleteEvent";
        public const string ResetBuildEventName = "ResetBuildEvent";
        #endregion
    }
}