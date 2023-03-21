using Firebase.Firestore;
using UnityEngine;

public class AchievementController 
{
    AchievementModel achievement;
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    public void ListAchievements ()
    {
        CollectionReference achievementRef = db.Collection("achievements");
        achievementRef.Document();
        Debug.Log(achievementRef);
    }
}