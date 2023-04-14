
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;
public class GameController
{
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;

    public void SavingGameToUser(string UserID, string GameID, int GameTask)
    {
        DocumentReference userRef = db.Collection("users").Document(UserID);
        DocumentReference gameRef = db.Collection("games").Document(GameID);

        userRef.GetSnapshotAsync().ContinueWith(async task =>
        {
            var userSnapshot = task.Result;
            if (userSnapshot.Exists)
            {
                UserModel user = userSnapshot.ConvertTo<UserModel>();
                int index = user.SavedGame.FindIndex((x) => x.GameID == GameID);

                if (index == -1)
                {
                    user.SavedGame.Add(new GameData()
                    {
                        GameID = GameID,
                        Task = GameTask,
                    });
                }
                else
                {
                    user.SavedGame[index].Task = GameTask;
                }
                ///////////////////////////////////////////
                var gameSnapshot = await gameRef.GetSnapshotAsync();

                GameModel game = gameSnapshot.ConvertTo<GameModel>();

                if (!user.User_ListAchievement.Contains(game.AchievementID[GameTask - 1]))
                {
                    user.User_ListAchievement.Add(game.AchievementID[GameTask - 1]);
                }
                await userRef.SetAsync(user, SetOptions.Overwrite);

            }
        });
    }

    public async Task<List<GameModel>> GetListGames(int unit, int chapter)
    {
        List<GameModel> listGames = new();
        string req = $"{unit},{chapter}";
        CollectionReference gamesRef = db.Collection("games");
        // Query query = gamesRef.WhereEqualTo("Unit", unit).WhereEqualTo("Chapter", chapter);


        Query query = gamesRef.WhereArrayContains("UnitChapter", req);
        var snapshots = await query.GetSnapshotAsync();
        foreach (DocumentSnapshot documentSnapshot in snapshots.Documents)
        {
            Debug.Log(String.Format("Document {0} returned", documentSnapshot.Id));
            listGames.Add(documentSnapshot.ConvertTo<GameModel>());
        }

        return listGames;
    }
}