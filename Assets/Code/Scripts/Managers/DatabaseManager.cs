using UnityEngine;
using MongoDB.Driver;


public class DatabaseManager : MonoBehaviour
{
    MongoClient client = new MongoClient(
        "mongodb+srv://khoa:khoa@cluster0.gwp7sx0.mongodb.net/?retryWrites=true&w=majority"
    );
    IMongoDatabase database;
    IMongoCollection<UserModel> collection;

    void Start() {
        database = client.GetDatabase("Math");
        collection = database.GetCollection<UserModel>("User");
        UserModel model;
     }
}
