using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using Project.Utils.ExtensionMethods;
using UnityEngine;
public class UserController
{
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;
    Firebase.Auth.FirebaseAuth auth => DatabaseManager.Auth;
    const string Collection = "users";
    UserModel user;

    public async Task<Firebase.Auth.FirebaseUser> RegisterAuth(string email, string password)
    {

        // Firebase.Auth.FirebaseUser newUser = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
        // UploadModel(newUser);
        // return newUser;
        Firebase.Auth.FirebaseUser newUser = null;
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            UploadModel(newUser);
        });
        return newUser;
    }
    public async Task<bool> SignInAuth(string email, string password)
    {
        Firebase.Auth.Credential credential =
        Firebase.Auth.EmailAuthProvider.GetCredential(email, password);

        Firebase.Auth.FirebaseUser user = null;
        await auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
        });
        ProfileUser(user.UserId);
        return user != null;
    }

    private void ProfileUser(string userID)
    {
        DocumentReference userDoc = db.Collection("users").Document(userID);
        userDoc.GetSnapshotAsync().ContinueWith(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            try{

                user = snapshot.ConvertTo<UserModel>();
                Debug.Log("ok");
                Debug.Log(user.User_ListAchievement.Count);
            }
            catch(Exception e){
                Debug.Log(e.Message);
            }
        });
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
    public void UploadModel(Firebase.Auth.FirebaseUser newUser)
    {
        user = new();
        // user.UserID = newUser.UserId;
        db.Collection("users").Document(newUser.UserId).SetAsync(user);

    }
}