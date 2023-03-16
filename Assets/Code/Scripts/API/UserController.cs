using UnityEngine;
using MongoDB.Driver;
using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;

public class UserController : MonoBehaviour
{
    IMongoDatabase database => DatabaseManager.Instance.Database;
    IMongoCollection<UserModel> userCollection;
    
    private void Start()
    {
        userCollection = database.GetCollection<UserModel>("User");
        database.RunCommand<BsonDocument>(new BsonDocument(){
            new Dictionary<string, object>(){
                {"drop","User"}
            }
        });
        var model = SendAsync("xmax", "pass").Result;
    }
    public async Task<UserModel> SendAsync(string username, string password)
    {
        UserModel user = new()
        {
            Username = username,
            Password = password
        };

        string created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        user.CreatedAt = created;
        user.UpdatedAt = created;
        return await AddUser(user);
    }
    private async Task<UserModel> AddUser(UserModel user)
    {
        await userCollection.InsertOneAsync(user);
        
        return user;
    }
}