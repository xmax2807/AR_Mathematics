using MongoDB.Bson;

public class UserModel
{
    public string UserID { get; set; }
    public string Username{get;set;}
    public string Password{get;set;}
    public  BsonTimestamp CreatedAt {get;set;}
    public BsonTimestamp UpdatedAt {get;set;}

}
