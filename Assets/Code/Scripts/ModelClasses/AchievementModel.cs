using Firebase.Firestore;
[FirestoreData]
public class AchievementModel{
    [FirestoreProperty] public string AchieveTitle {get; set;}
    [FirestoreProperty] public string AchieveImg {get; set;}
    [FirestoreProperty] public string AchieveStatus {get; set;}

    
}