using UnityEngine;
using Project.AssetIO;
using System.Linq;

namespace Project.SaveLoad
{
    public class InGameSaveLoadManager : MonoBehaviour
    {
        public static InGameSaveLoadManager Instance { get; private set; }
        private IFileHandler FileIO;
        private string _savePath;
        private GameData gameData;
        private ISavable[] cache;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void SaveGame() {
            cache = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToArray();
            foreach(ISavable obj in cache){
                obj.Save(gameData);
            }
            FileIO.Write(gameData, _savePath);
        }
        public void LoadGame(){
            gameData = FileIO.Read<GameData>(_savePath);
            cache = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToArray();
            foreach(ISavable obj in cache){
                obj.Load(gameData);
            }
        }
    }
}