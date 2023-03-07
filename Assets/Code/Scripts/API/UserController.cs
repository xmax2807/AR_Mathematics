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
        Send("xmax", "pass");
    }
    public  UserModel Send(string username, string password)
    {
        UserModel user = new UserModel();
        user.Username = username;
        user.Password = password;

        string created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        user.CreatedAt = created;
        user.UpdatedAt = created;
        return AddUser(user);
        
    }
    private  UserModel AddUser(UserModel user)
    {
         userCollection.InsertOneAsync(user);
        
        return user;
    }
}