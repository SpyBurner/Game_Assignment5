using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Unity.VisualScripting;
using System.Reflection;

public class FirebaseManager : PersistentSingleton<FirebaseManager>
{
    private FirebaseAuth auth;

    private FirebaseFirestore firestore;

    private Player playerInfo;

    void Start()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                this.auth = FirebaseAuth.DefaultInstance;
                this.firestore = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError($"Failed to initialize Firebase: {task.Exception}");
            }
        });
    }

    public FirebaseUser GetCurrentUser()
    {
        return this.auth.CurrentUser;
    }

    public void RegisterAccount(string email, string password, System.Action<bool> callback)
    {
        this.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("RegisterAccount was canceled.");
                callback(false);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("RegisterAccount encountered an error: " + task.Exception);
                callback(false);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.Log($"User  registered successfully: {result.User.Email}");
            callback(true);
        });
    }

    public void Login(string email, string password, System.Action<bool> callback)
    {
        this.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("Login was canceled.");
                callback(false);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Login encountered an error: " + task.Exception);
                callback(false);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.Log($"User  logged in successfully: {result.User.Email}");
            callback(true);
        });
    }

    public void Logout()
    {
        this.auth.SignOut();
        Debug.Log("User  logged out.");
    }

    public void OnEnable()
    {
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void OnDisable()
    {
        auth.StateChanged -= AuthStateChanged;
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser  != null)
        {
            Debug.Log("User  is signed in: " + auth.CurrentUser .Email);
        }
        else
        {
            Debug.Log("No user is signed in.");
        }
    }

    public void InitializePlayerInfo(System.Action<Player> callback)
    {
        if (this.auth == null || this.auth.CurrentUser == null)
        {
            Debug.LogWarning("Cannot initialize player info. User is not logged in.");
            callback(null);
            return;
        }
        
        string userId = this.auth.CurrentUser.UserId;
        firestore.Collection("users").Document(userId).GetSnapshotAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("InitializePlayerInfo was canceled.");
                callback(null);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("InitializePlayerInfo encountered an error: " + task.Exception);
                callback(null);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                playerInfo = snapshot.ConvertTo<Player>();
                callback(playerInfo);
            }
            else
            {
                Debug.LogError("Player info does not exist.");
                callback(null);
            }
        });
    }

    public void UpdatePlayerInfo(Player newInfo, System.Action<bool> callback) {
        if (this.auth == null || this.auth.CurrentUser == null) {
            Debug.LogWarning("Cannot update player info. User is not logged in.");
            callback(false);
            return;
        }

        Dictionary<string, object> data = newInfo.ConvertToDictionary();

        string userId = this.auth.CurrentUser.UserId;
        firestore.Collection("users").Document(userId).SetAsync(data).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("UpdatePlayerInfo was canceled.");
                callback(false);
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("UpdatePlayerInfo encountered an error: " + task.Exception);
                callback(false);
                return;
            }

            this.playerInfo.SetFromDictionary(data);
            callback(true);
        });
    }
}