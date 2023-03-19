using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using Project.Utils.ExtensionMethods;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
public class UserController
{
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;
    private void Register(string Username, string Password)
    {

        if (CheckIfDocumentExist("users", Username)) return;

        Debug.Log("Username available");

        string encryptPassword = StringExtensionMethods.EncryptString(Username + Password, Password);

        db.Collection("users").Document(Username).SetAsync(new UserModel()
        {
            Password = encryptPassword
        }).ContinueWith(task =>
        {
            Debug.Log("Added document with ID: " + Username);
        });

    }
    public bool CheckIfDocumentExist(string collection, string document)
    {
        var documentSnapshot = db.Collection(collection).Document(document).GetSnapshotAsync().Result;

        if (documentSnapshot.Exists)
        {
            Debug.Log(document + " exists in " + collection);
        }

        return documentSnapshot.Exists;
    }
    public async Task<T> GetValueAsync<T>(string collection, string document)
    {

        DocumentSnapshot doc = await db.Collection(collection).Document(document).GetSnapshotAsync();        

        if (doc == null || !doc.Exists)
        {

            return default;
        }

        return doc.ConvertTo<T>();
    }
    public async void SignIn(string username, string password)
    {

        var model = await GetValueAsync<UserModel>("users", username);
        if (model == null)
        {
            Debug.Log("Username is not exist!");
            return;
        }

        string decryptPassword = StringExtensionMethods.DecryptString(username + password, model.Password);

        if (decryptPassword == password)
        {
            Debug.Log("Login success");
        }
        else
        {
            Debug.Log("Incorrect password");
            return;
        }

    }
    
    // Setup authentication event handlers if desired
    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
          {
              Debug.Log("Player session could not be refreshed and expired.");
          };
    }
    public async Task SignInAnonymouslyAsync()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        SetupEvents();
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}