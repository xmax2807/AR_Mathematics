using Firebase.Firestore;
[FirestoreData]
public class GameModel
{
    [FirestoreProperty] public string GameTitle { get; set; }
    [FirestoreProperty] public string LessonID { get; set; }
    [FirestoreProperty] public string[] AchievementID { get; set; }
    [FirestoreProperty] public int[] Task { get; set; }

}