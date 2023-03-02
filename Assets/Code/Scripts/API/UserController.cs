using UnityEngine;
using MongoDB.Driver;
using System;

public class UserController : MonoBehaviour
{
    MongoClient client = new MongoClient(
       "mongodb+srv://khoa:khoa@cluster0.gwp7sx0.mongodb.net/?retryWrites=true&w=majority"
   );
    IMongoDatabase database;
    IMongoCollection<UserModel> userCollection;

    private void Start()
    {
        System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();
        database = client.GetDatabase("Math");
        userCollection = database.GetCollection<UserModel>("User");
        Send("xmax", "pass");
    }
    public void Send(string username, string password)
    {
        UserModel user = new UserModel();
        user.Username = username;
        user.Password = password;
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;

        AddUser(user);
    }
    private async void AddUser(UserModel user)
    {
        await userCollection.InsertOneAsync(user);
    }
}