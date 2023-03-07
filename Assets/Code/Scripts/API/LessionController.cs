using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
public class LessionController : MonoBehaviour{
    MongoClient client = new MongoClient("mongodb+srv://khoa:khoa@cluster0.gwp7sx0.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<LessonModel> LessionCollection;
    
}