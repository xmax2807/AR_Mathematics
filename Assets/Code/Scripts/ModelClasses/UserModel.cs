using MongoDB.Bson;

public class UserModel : RealmObject
{
    public string UserID { get; set; }
    public string Username{get;set;}
    public string Password{get;set;}
    // public  CreatedAt {get;set;}
    // public string UpdatedAt {get;set;}

}
