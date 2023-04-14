using Firebase.Firestore;
[FirestoreData]
public class GameModel
{
    [FirestoreProperty] public string GameTitle { get; set; } 
    [FirestoreProperty] public string[] UnitChapter { get; set; }
    [FirestoreProperty] public string GameScene { get; set; }

    [FirestoreProperty] public string[] AchievementID { get; set; }
    [FirestoreProperty] public int[] Task { get; set; }



}