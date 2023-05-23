using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AchievementController
{
    AchievementModel achievement;
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    public void Upload()
    {
        db.Collection("achievements").Document().SetAsync(new AchievementModel()
        {
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
                if (!documentSnapshot.Exists)
                {
                    Debug.Log("Not Existed");
                }
                try
                {

                    achievement = documentSnapshot.ConvertTo<AchievementModel>();
                    Debug.Log(achievement.AchieveTitle);
                }
                catch (FirestoreException e)
                {
                    Debug.Log(e.Message);
                }
                //achievement = achievements.Values.Select((x)=>).ToArray();
            }
        });
    }

    public async Task<AchievementModel[]> GetAchievementsOfUser(UserModel user)
    {
        if (user == null) return null;

        List<AchievementModel> result = new(8);

        Query query = db.Collection("achievements");
        QuerySnapshot snapshots = await query.GetSnapshotAsync();
        Debug.Log(user.User_ListAchievement.Count);
        foreach (DocumentSnapshot documentSnapshot in snapshots)
        {
            try
            {
                Debug.Log(documentSnapshot.Id);
                if (!user.User_ListAchievement.Contains(documentSnapshot.Id)) continue;

                AchievementModel model = documentSnapshot.ConvertTo<AchievementModel>();
                result.Add(model);
            }
            catch (FirestoreException e)
            {
                Debug.Log(e.Message);
            }
        }
        return result.ToArray();
    }
}