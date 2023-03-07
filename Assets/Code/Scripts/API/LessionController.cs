using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
public class LessionController : MonoBehaviour{
    IMongoDatabase database => DatabaseManager.Instance.Database;

    IMongoCollection<LessonModel> LessionCollection;
    
}