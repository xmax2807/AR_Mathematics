using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class GameData
{
    // [FirestoreProperty] public string Username { get; set; }
    [FirestoreProperty]public string GameID { get; set; }
    [FirestoreProperty]public int Task { get; set; }

}
[FirestoreData]
public class UserModel
{
    // [FirestoreProperty] public string Username { get; set; }

    // [FirestoreProperty] public string UserID { get; set; }
    [FirestoreProperty] public List<string> User_ListAchievement { get; set; }
    [FirestoreProperty] public List<GameData> SavedGame { get; set; }

    public UserModel(){
        User_ListAchievement = new(1);
        SavedGame = new(1);
    }

}