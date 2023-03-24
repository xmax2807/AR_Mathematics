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

    public async Task<Firebase.Auth.FirebaseUser> RegisterAuth(string email, string password)
    {
        return await auth.CreateUserWithEmailAndPasswordAsync(email, password);
    }
    public async Task<bool> SignInAuth(string email, string password)
    {
        Firebase.Auth.Credential credential =
    Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
        var user = await auth.SignInWithCredentialAsync(credential);
        return user != null;
    }
    UserModel user;
    public void UploadModel(Firebase.Auth.FirebaseUser newUser)
    {
        user = new();
        // user.UserID = newUser.UserId;
        db.Collection("users").Document(newUser.UserId).SetAsync(user);

    }
}