using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AchievementController
{
    AchievementModel achievement;
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    public void Upload(){
        db.Collection("achievements").Document().SetAsync(new AchievementModel(){
            AchieveTitle = "asdsad",
            AchieveStatus = true,
        });
    }
    public void ListAchievements()
    {
        Query AchievementListQuery = db.Collection("achievements");
        AchievementListQuery.GetSnapshotAsync().ContinueWith(task =>
        {
            QuerySnapshot allCitiesQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
            {
                Debug.Log(string.Format("Document data for {0} document:", documentSnapshot.Id));
                // Dictionary<string, object> achievements = documentSnapshot.ToDictionary();
                if(!documentSnapshot.Exists){
                    Debug.Log("Not Existed");
                }
                try{

                    achievement = documentSnapshot.ConvertTo<AchievementModel>();
                    Debug.Log(achievement.AchieveTitle);
                }
                catch(FirestoreException e){
                    Debug.Log(e.Message);                  
                }
                //achievement = achievements.Values.Select((x)=>).ToArray();
            }
        });
    }
}