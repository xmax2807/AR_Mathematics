namespace Project.SaveLoad{

    /// <summary>
    /// Interface for Savable objects
    /// </summary>
    public interface ISavable{
        void Save(GameData gameData);
        void Load(GameData gameData);
    }
}
