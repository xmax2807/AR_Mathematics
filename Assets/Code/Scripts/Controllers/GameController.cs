
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;
public class GameController
{
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;

    public void SavingGameToUser(string UserID, string GameID, int GameTask)
    {
        DocumentReference userRef = db.Collection("users").Document(UserID);
        // GameData savingGame = new();
        // savingGame.GameID = GameID;
        // savingGame.Task = GameTask;

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
                var gameSnapshot = await gameRef.GetSnapshotAsync();

                GameModel game = gameSnapshot.ConvertTo<GameModel>();

                if (!user.User_ListAchievement.Contains(game.AchievementID[GameTask - 1]))
                {
                    user.User_ListAchievement.Add(game.AchievementID[GameTask - 1]);
                }
                await userRef.SetAsync(user, SetOptions.Overwrite);

            }
        });

        // Query queryUser = db.Collection("users").WhereEqualTo("UserID", UserID);
        // DocumentReference docGame = db.Collection("games").Document(GameID);

        // GameModel game = new();
        // docGame.GetSnapshotAsync().ContinueWith(task =>
        // {
        //     DocumentSnapshot gameSnapshot = task.Result;
        //     game = gameSnapshot.ConvertTo<GameModel>();
        //     Debug.Log(game.GameTitle);
        //     queryUser.GetSnapshotAsync().ContinueWith(task =>
        // {
        //     QuerySnapshot userSnapshot = task.Result;
        //     var user = userSnapshot[0].ConvertTo<UserModel>();
        //     GameData savingGame = new()
        //     {
        //         GameID = GameID,
        //         Task = GameTask
        //     };
        //     Debug.Log(savingGame.GameID);
        //     DocumentReference userDoc = db.Collection("users").Document(UserID);
        //     Dictionary<string, object> update = new()
        //     {
        //         {"GameID",GameID},
        //         {"Task",GameTask}
        //     };
        //     user.SavedGame.Add(savingGame);
        //     for (int i = 0; i < GameTask; i++)
        //     {
        //         user.User_ListAchievement.Add(game.AchievementID[GameTask - 1 - i]);

        //     }

        // });
        // });

    }
}