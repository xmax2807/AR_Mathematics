using System.Collections.Generic;

[System.Serializable]
public class UserLocalModel {
    public string UserID {get;set;}
    public List<string> ListOfAcquiredARModel {get;set;}

    [Newtonsoft.Json.JsonConstructor]
    public UserLocalModel(string userId, List<string> acquiredARModel){
        UserID = userId;

        if(acquiredARModel == null){
            ListOfAcquiredARModel = new List<string>();
        }
        else{
            ListOfAcquiredARModel = new List<string>(acquiredARModel);
        }
    }
    public UserLocalModel(string id){
        UserID = id;
        ListOfAcquiredARModel = new List<string>(){
            // "Wibu",
            // "Octopus",
            // "Pen",
            // "Pencil",
            // "PenCase",
            // "Gull",
            // "Eraser",
        };
    }
}