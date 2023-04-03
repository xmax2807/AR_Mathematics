using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using Project.Utils.ExtensionMethods;
using UnityEngine;
public class UserController
{
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;
    Firebase.Auth.FirebaseAuth auth => DatabaseManager.Auth;
    public Firebase.Auth.FirebaseUser FireBaseUser {get;private set;}
    const string Collection = "users";
    public UserController()
    {
        auth.StateChanged += AuthStateChanged;
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != FireBaseUser)
        {
            bool signedIn = FireBaseUser != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && userModel != null)
            {
                Debug.Log("Signed out " + FireBaseUser.UserId);
            }
            FireBaseUser = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + FireBaseUser.UserId);
            }
        }
    }

    public async Task<Firebase.Auth.FirebaseUser> RegisterAuth(string email, string password)
    {
        return await auth.CreateUserWithEmailAndPasswordAsync(email, password);
    }
    public async Task<bool> SignInAuth(string email, string password)
    {
        Firebase.Auth.Credential credential =
        Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
        var user = await auth.SignInWithCredentialAsync(credential);

        if(user != null){
            userModel = await GetUserModel(user.UserId);
        }
        return user != null;
    }
    public void SignOutAuth()
    {
        try
        {
            auth.SignOut();
        }
        catch (Firebase.FirebaseException e)
        {
            Debug.Log(e.Message);
        }
    }
    UserModel userModel;
    public void UploadModel(Firebase.Auth.FirebaseUser newUser)
    {
        userModel = new();
        // user.UserID = newUser.UserId;
        db.Collection("users").Document(newUser.UserId).SetAsync(userModel);

    }

    public async Task<UserModel> GetUserModel(string userId){
        DocumentSnapshot doc = await db.Collection("users").Document(userId).GetSnapshotAsync();
        return doc.ConvertTo<UserModel>();
    }
}