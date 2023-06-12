
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using Project.Managers;
using Project.Utils.ExtensionMethods;
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

    public async void SaveGame(int task)
    {
        UserModel user = UserManager.Instance.CurrentUser;
        if (user == null) return;

        GameModel game = UserManager.Instance.CurrentGame;
        string gameId = game?.GameID;
        int index = user.SavedGame == null ? -1 : user.SavedGame.FindIndex((x) => x.GameID == gameId);

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

        if (task - 1 >= game.AchievementID.Length)
        {
            UnityEngine.Debug.Log("Save without Achivement");
            await DatabaseManager.Instance.UserController.UpdateUserGameData(user);
            return;
        }

        if (!user.User_ListAchievement.Contains(game.AchievementID[task - 1]))
        {
            user.User_ListAchievement.Add(game.AchievementID[task - 1]);
        }

        await DatabaseManager.Instance.UserController.UpdateUserGameData(user);
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
        int chapIndex = chapter - 1;
        if (chapIndex < 0) return null;

        CollectionReference gamesRef = db.Collection("games");
        List<GameModel> listGames = new();

        int[] chapCounts = UserManager.Instance.CourseModel.chapCount.ToArray();

        if (chapIndex >= chapCounts.Length) return null;

        int maxChapCount = chapCounts[chapIndex];

        string[] req = new string[maxChapCount];
        for (int i = 1; i <= maxChapCount; i++)
        {
            req[i - 1] = $"{i},{chapter}";
        }

        Query query = gamesRef.WhereArrayContainsAny("UnitChapter", req);

        var snapshots = await query.GetSnapshotAsync();
        foreach (DocumentSnapshot documentSnapshot in snapshots.Documents)
        {
            GameModel model = documentSnapshot.ConvertTo<GameModel>();
            Debug.Log(model.GameTitle);
            listGames.Add(model);
        }

        UserManager.Instance.CurrentUnitProgress = new UserManager.CurrentUnit()
        {
            chapter = chapter,
            unit = -1,
        };
        Debug.Log("Game chapter: " + UserManager.Instance.CurrentUnitProgress.chapter);

        return listGames;
    }

    public int GetLastSavedTask(string gameId)
    {
        Debug.Log(gameId);
        UserModel currentUser = UserManager.Instance.CurrentUser;
        if (currentUser == null) return 0;

        GameData data = currentUser.SavedGame.FindMatch((x) => x.GameID == gameId);
        if (data == null) return 0;
        return data.Task;
    }
}