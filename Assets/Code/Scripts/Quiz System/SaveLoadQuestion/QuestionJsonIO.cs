using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.AssetIO;
using Project.Managers;

namespace Project.QuizSystem.SaveLoadQuestion{
    public class QuestionJsonIO{
        private UserModel CurrentUser => UserManager.Instance.CurrentUser;
        private readonly IFileHandler fileHandler;
        private readonly string filePath;
        private const string folder = "student_tests";
        public QuestionJsonIO(){
            
            filePath = Path.Combine(UnityEngine.Application.persistentDataPath, folder);

            JsonSerializerSettings settings = new() { TypeNameHandling = TypeNameHandling.All };
            fileHandler = new JsonFileHandler(settings);
        }
        public QuestionJsonIO(IFileHandler fileHandler){
            filePath = Path.Combine(UnityEngine.Application.persistentDataPath, folder); 
            this.fileHandler = fileHandler;
        }

        public Task SaveSemester(SemesterTestSaveData data){
            string userId = "test";
            if(CurrentUser != null){
                userId = CurrentUser.UserID;
            }
            else{
                //throw new System.NullReferenceException("User does not exist");
            }

            data.UserId = userId;
            string fullPath = Path.Combine(filePath, userId, $"{data.Id}.json");
            return fileHandler.WriteAsync(data, fullPath);
        }
        public Task<SemesterTestSaveData> LoadSemester(int semester){
            string userId = "test";
            if(CurrentUser != null){
                userId = CurrentUser.UserID;
            }
            else{
                //throw new System.NullReferenceException("User does not exist");
            }
            string fullPath = Path.Combine(filePath, userId, $"{userId}_{semester}.json");
            return fileHandler.ReadAsync<SemesterTestSaveData>(fullPath);
        }
    }
}