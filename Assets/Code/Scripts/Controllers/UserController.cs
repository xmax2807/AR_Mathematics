using System;
using System.IO;
using System.Threading.Tasks;
using Firebase.Firestore;
using Firebase.Storage;
using Project.Managers;
using Project.Utils.ExtensionMethods;
using RobinBird.FirebaseTools.Storage.Addressables;
using UnityEngine;

public struct AuthResult
{
    public bool IsSuccessful;
    public string ErrorMessage;
    public AuthResult(bool isSuccessful = false, string errorMessage = "")
    {
        this.IsSuccessful = isSuccessful;
        this.ErrorMessage = errorMessage;
    }
}
public class UserController
{
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;
    Firebase.Auth.FirebaseAuth auth => DatabaseManager.Auth;
    public Firebase.Auth.FirebaseUser FireBaseUser { get; private set; }
    private Project.AssetIO.JsonFileHandler m_localFileHandler;
    const string Collection = "users";
    public UserController()
    {
        auth.StateChanged += AuthStateChanged;
        m_localFileHandler = new Project.AssetIO.JsonFileHandler();
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

    public async Task<(Firebase.Auth.FirebaseUser, AuthResult)> RegisterAuth(string email, string password)
    {

        // Firebase.Auth.FirebaseUser newUser = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
        // UploadModel(newUser);
        // return newUser;
        Firebase.Auth.FirebaseUser newUser = null;
        AuthResult authResult = new AuthResult(true);
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            string errorMessage = "Lỗi không xác định";
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                authResult = new(false, errorMessage);
                return;
            }
            if (task.IsFaulted)
            {

                //Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                Firebase.FirebaseException exception = (Firebase.FirebaseException)task.Exception.InnerExceptions[0].InnerException;
                if (exception == null)
                {
                    authResult = new AuthResult(false, errorMessage);
                    return;
                }

                Debug.Log(exception.ErrorCode);
                switch ((Firebase.Auth.AuthError)exception.ErrorCode)
                {
                    case Firebase.Auth.AuthError.UserNotFound:
                        errorMessage = "Tài khoản không tồn tại";
                        break;
                    case Firebase.Auth.AuthError.WrongPassword:
                        errorMessage = "Sai mật khẩu";
                        break;
                    case Firebase.Auth.AuthError.InvalidEmail:
                        errorMessage = "Email không hợp lệ";
                        break;
                    case Firebase.Auth.AuthError.EmailAlreadyInUse:
                        errorMessage = "Email đã tồn tại";
                        break;
                    case Firebase.Auth.AuthError.NetworkRequestFailed:
                        errorMessage = "Mất kết nối tới máy chủ";
                        break;
                    case Firebase.Auth.AuthError.WeakPassword:
                        errorMessage = "Sai mật khẩu";
                        break;
                    default:
                        errorMessage = "Lỗi không xác định";
                        break;
                }
                authResult = new AuthResult(false, errorMessage);
                return;
            }

            // Firebase user has been created.
            newUser = task.Result.User;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            UploadModel(newUser);
            //CreateSaveData(newUser.UserId);
        });
        return (newUser, authResult);
    }
    public async Task<AuthResult> SignInAuth(string email, string password)
    {
        Firebase.Auth.Credential credential =
        Firebase.Auth.EmailAuthProvider.GetCredential(email, password);

        Firebase.Auth.FirebaseUser user = null;

        AuthResult authResult = new AuthResult(true);
        string errorMessage = "Lỗi không xác định";
        await auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {

            if (task.IsCanceled)
            {
                //Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                authResult = new AuthResult(false);
                return;
            }
            if (task.IsFaulted)
            {
                //Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                Firebase.FirebaseException exception = (Firebase.FirebaseException)task.Exception.InnerExceptions[0].InnerException;
                if (exception == null)
                {
                    authResult = new AuthResult(false, errorMessage);
                    return;
                }

                Debug.Log(exception.ErrorCode);
                switch ((Firebase.Auth.AuthError)exception.ErrorCode)
                {
                    case Firebase.Auth.AuthError.UserNotFound:
                        errorMessage = "Tài khoản không tồn tại";
                        break;
                    case Firebase.Auth.AuthError.WrongPassword:
                        errorMessage = "Sai mật khẩu";
                        break;
                    case Firebase.Auth.AuthError.InvalidEmail:
                        errorMessage = "Email không hợp lệ";
                        break;
                    case Firebase.Auth.AuthError.EmailAlreadyInUse:
                        errorMessage = "Email đã tồn tại";
                        break;
                    case Firebase.Auth.AuthError.NetworkRequestFailed:
                        errorMessage = "Mất kết nối tới máy chủ";
                        break;
                    case Firebase.Auth.AuthError.WeakPassword:
                        errorMessage = "Sai mật khẩu";
                        break;
                    default:
                        errorMessage = "Lỗi không xác định";
                        break;
                }
                authResult = new AuthResult(false, errorMessage);
                return;
            }

            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
        });

        if (user == null) return authResult;

        await GameManager.Instance.CheckIsAdminLoggedIn(user.UserId);
        FirebaseAddressablesManager.IsFirebaseSetupFinished = true;
        ProfileUser(user.UserId);
        //UserManager.Instance.CurrentLocalUser = GetLocalUserModel(user.UserId);
        return authResult;
    }

    private void ProfileUser(string userID)
    {
        Query query = db.Collection("users").WhereEqualTo("UserID", userID);
        query.GetSnapshotAsync().ContinueWith(task =>
        {
            try
            {
                DocumentSnapshot snapshot = task.Result[0];

                userModel = snapshot.ConvertTo<UserModel>();
                UserManager.Instance.CurrentUser = userModel;
                //Debug.Log("ok");
            }
            catch (Exception e)
            {
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
    UserModel userModel;
    public void UploadModel(Firebase.Auth.FirebaseUser newUser)
    {
        userModel = new()
        {
            UserID = newUser.UserId,
        };
        // user.UserID = newUser.UserId;
        db.Collection("users").Document(newUser.UserId).SetAsync(userModel);
    }

    private void CreateSaveData(string userId)
    {
        UserLocalModel localModel = new(userId);
        m_localFileHandler.Write(localModel, Path.Combine(Application.persistentDataPath, $"userLocalSave/{userId}.json"));
    }
    public UserLocalModel GetLocalUserModel(string userId)
    {
        UserLocalModel result = m_localFileHandler.Read<UserLocalModel>(Path.Combine(Application.persistentDataPath, $"userLocalSave/{userId}.json"));
        if (result == null)
        {
            CreateSaveData(userId);
        }
        return result;
    }

    public async Task<UserModel> GetUserModel(string userId)
    {
        DocumentSnapshot doc = await db.Collection("users").Document(userId).GetSnapshotAsync();
        return doc.ConvertTo<UserModel>();
    }

    public void SaveLocalData()
    {
        m_localFileHandler.Write<UserLocalModel>(UserManager.Instance.CurrentLocalUser, Path.Combine(Application.persistentDataPath, $"userLocalSave/{UserManager.Instance.CurrentUser.UserID}.json"));
    }

    public async Task<bool> ReAuthenticateWithCredential(string email, string password)
    {
        var credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);

        try
        {
            var signInResult = await auth.CurrentUser.ReauthenticateAndRetrieveDataAsync(credential);
            return signInResult != null;
        }
        catch (Firebase.FirebaseException e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    public async Task UpdateUser(UserModel user)
    {
        try
        {
            DocumentReference userRef = db.Collection("users").Document(user.UserID);
            await userRef.SetAsync(user, SetOptions.Overwrite);

        }
        catch (Firebase.FirebaseException e)
        {
            Debug.Log(e.Message);
        }
    }

    public async Task DeleteCurrentUser()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            string email = user.Email;
            DocumentReference userRef = db.Collection(Collection).Document(user.UserId);
            Task deleteTask = userRef.DeleteAsync();
            await deleteTask;

            if (deleteTask.IsFaulted)
            {
                Debug.Log("Delete User: " + deleteTask.Exception.Message);
                return;
            }

            deleteTask = user.DeleteAsync();
            await deleteTask;

            Debug.Log("Delete completed");
            if (deleteTask.IsFaulted)
            {
                Debug.Log("Delete User: " + deleteTask.Exception.Message);
            }
            else
            {
                Debug.Log($"Delete user with {email} successfully");
            }
        }
    }
    public async Task<Uri> UploadImageBytes(string userId, byte[] contentBytes)
    {
        string guid = Guid.NewGuid().ToString();
        StorageReference folderRef = DatabaseManager.Storage.RootReference.Child($"user_images/{userId}/{userId}_{guid}.png");

        // Upload the file to the path "images/rivers.jpg"
        Task<StorageMetadata> task = folderRef.PutBytesAsync(contentBytes);
        await task;
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.Log(task.Exception.ToString());
            // Uh-oh, an error occurred!
            return new Uri("");
        }
        else
        {
            // Metadata contains file metadata such as size, content-type, and md5hash.
            //StorageMetadata metadata = task.Result;
            return await folderRef.GetDownloadUrlAsync();
            // string filePath = metadata.Path;
            // Debug.Log("Finished uploading...");
            // Debug.Log("file path = " + filePath);
            // Debug.Log("file size = " + metadata.SizeBytes);
        }
    }
}