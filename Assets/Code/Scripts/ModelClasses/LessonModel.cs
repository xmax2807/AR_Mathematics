using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

public class LessonModel : MonoBehaviour
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string LessionTitle { get; set; }
    public string LessionVideo { get; set; }
    public int LessionChapter { get; set; }
    public int LessionSemester { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
}