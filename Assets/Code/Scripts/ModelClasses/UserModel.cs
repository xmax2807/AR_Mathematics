using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class UserModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Username{get;set;}
    public string Password{get;set;}
    public string CreatedAt {get;set;}
    public string UpdatedAt {get;set;}

}
