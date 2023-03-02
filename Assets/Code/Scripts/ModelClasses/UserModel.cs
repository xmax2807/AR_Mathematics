using MongoDB.Bson;

public class UserModel
{
    // public string UserID { get; set; }
    public string Username{get;set;}
    public string Password{get;set;}
    public  BsonDateTime CreatedAt {get;set;}
    public BsonDateTime UpdatedAt {get;set;}

}
