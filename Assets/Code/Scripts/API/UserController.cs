using UnityEngine;
using MongoDB.Driver;
using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserController : MonoBehaviour
{
    MongoClient client = new MongoClient(
       "mongodb+srv://khoa:khoa@cluster0.gwp7sx0.mongodb.net/?retryWrites=true&w=majority"
   );
    IMongoDatabase database;
    IMongoCollection<UserModel> userCollection;

    private void Start()
    {
        database = client.GetDatabase("Math");
        userCollection = database.GetCollection<UserModel>("User");
        database.RunCommand<BsonDocument>(new BsonDocument(){
            new Dictionary<string, object>(){
                {"drop","User"}
            }
        });    
        Send("xmax", "pass");
    }
    public UserModel Send(string username, string password)
    {
        UserModel user = new UserModel();
        user.Username = username;
        user.Password = password;

        string created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        user.CreatedAt = created;
        user.UpdatedAt = created;
        return AddUser(user).Result;
        
    }
    private async Task<UserModel> AddUser(UserModel user)
    {
        await userCollection.InsertOneAsync(user);
        
        return user;
    }
}