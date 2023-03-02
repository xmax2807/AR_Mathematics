using UnityEngine;
using MongoDB.Driver;

public class UserController : MonoBehaviour {
     MongoClient client = new MongoClient(
        "mongodb+srv://khoa:khoa@cluster0.gwp7sx0.mongodb.net/?retryWrites=true&w=majority"
    );
    IMongoDatabase database;
    IMongoCollection<UserModel> userCollection;

    private UserModel addUser(string rawJson){
        var newUser = new UserModel();
        return newUser;
    }
}