
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using Project.Managers;
using Project.Utils.ExtensionMethods;
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

    public async void SaveGame(int task)
    {
        UserModel user = UserManager.Instance.CurrentUser;
        GameModel game = UserManager.Instance.CurrentGame;
        string gameId = game.GameId;
        int index = user.SavedGame.FindIndex((x) => x.GameID == gameId);

        if (index == -1)
        {
            user.SavedGame.Add(new GameData()
            {
                GameID = gameId,
                Task = task,
            });
        }
        else
        {
            user.SavedGame[index].Task = task;
        }
        if (!user.User_ListAchievement.Contains(game.AchievementID[task - 1]))
        {
            user.User_ListAchievement.Add(game.AchievementID[task - 1]);
        }

        DocumentReference userRef = db.Collection("users").Document(user.UserID);
        await userRef.SetAsync(user, SetOptions.Overwrite);
    }

    public async Task<List<GameModel>> GetListGames(int unit, int chapter)
    {
        if (unit == -1)
        {
            return await GetListGamesInChapter(chapter);
        }

        List<GameModel> listGames = new();

        CollectionReference gamesRef = db.Collection("games");
        Query query = gamesRef.WhereArrayContains("UnitChapter", $"{unit},{chapter}");
        var snapshots = await query.GetSnapshotAsync();

        foreach (DocumentSnapshot documentSnapshot in snapshots.Documents)
        {
            listGames.Add(documentSnapshot.ConvertTo<GameModel>());
        }

        return listGames;
    }
    public async Task<List<GameModel>> GetListGamesInChapter(int chapter)
    {
        CollectionReference gamesRef = db.Collection("games");
        List<GameModel> listGames = new();

        if (chapter >= UserManager.Instance.CourseModel.chapCount.Count) return null;
        int maxChapCount = UserManager.Instance.CourseModel.chapCount[chapter];
        string[] req = new string[maxChapCount];
        for (int i = 1; i < maxChapCount; i++)
        {
            req[i - 1] = $"{i},{chapter}";
        }

        Query query = gamesRef.WhereArrayContainsAny("UnitChapter", req);

        var snapshots = await query.GetSnapshotAsync();
        foreach (DocumentSnapshot documentSnapshot in snapshots.Documents)
        {
            //            Debug.Log(String.Format("Document {0} returned", documentSnapshot.Id));
            listGames.Add(documentSnapshot.ConvertTo<GameModel>());
        }

        return listGames;
    }

    public int GetLastSavedTask(string gameId)
    {
        UserModel currentUser = UserManager.Instance.CurrentUser;
        if (currentUser == null) return 0;

        GameData data = currentUser.SavedGame.FindMatch((x) => x.GameID == gameId);
        if (data == null) return 0;
        return data.Task;
    }
}