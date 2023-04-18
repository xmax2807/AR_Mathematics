using UnityEngine.ResourceManagement.ResourceLocations;
using Firebase.Storage;

public class FirebaseSignedUrl{
    private readonly string internalId;
    public FirebaseSignedUrl(string internalId){
        this.internalId = internalId;
    }
    public string ReplaceInteralId(IResourceLocation location){
        if(!location.InternalId.StartsWith(internalId)) return location.InternalId;

        string oldUrl = location.InternalId;
        UnityEngine.Debug.Log(oldUrl);
        StorageReference reference = DatabaseManager.Storage.GetReference(oldUrl);
        UnityEngine.Debug.Log(reference.Path);
        return location.InternalId.Replace(oldUrl, reference.Path);
    }
}