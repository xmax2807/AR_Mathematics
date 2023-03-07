using UnityEngine;
using MongoDB.Driver;


public class DatabaseManager : MonoBehaviour
{   
    public static DatabaseManager Instance {get;private set;}
    public MongoClient Client {get;private set;}
    private void Awake(){
        if(Instance != null) return;

        Instance = this;
        PlayerPrefs.SetString("DBConnection", "mongodb+srv://khoa:khoa@cluster0.gwp7sx0.mongodb.net/?retryWrites=true&w=majority");
        
        Instance.Client = new MongoClient(
            PlayerPrefs.GetString("DBConnection")
        ); 

        
        Database = Client.GetDatabase("Math");
        collection = Database.GetCollection<UserModel>("User");
    }
    public IMongoDatabase Database {get;private set;}
    IMongoCollection<UserModel> collection;
}
