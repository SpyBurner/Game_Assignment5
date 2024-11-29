using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using System.Security.Authentication;

public class PlayFabManager : PersistentSingleton<PlayFabManager>
{
    private string playFabId;
    public Dictionary<string, string> playerData = new Dictionary<string, string>();
    public bool isLoggedIn = false;

    private void Start()
    {

    }

    private void AuthenticateWithPlayFab()
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        }, OnLoginSuccess, OnLoginError);
    }

    public void RegisterWithEmailAndPassword(string displayName, string email, string password)
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            DisplayName = displayName?? "",
            Email = email?? "",
            Password = password?? "",
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        if (result != null && result.PlayFabId != null)
        {
            playFabId = result.PlayFabId;
            Debug.Log("Registration successful. Player ID: " + playFabId);
        }
        else
        {
            Debug.Log("Registration failed: " + result);
        }
    }

    private void OnRegisterError(PlayFabError error)
    {
        Debug.LogError("Error registering into PlayFab: " + error.GenerateErrorReport());
    }

    public void LoginWithEmailAndPassword(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        playFabId = result.PlayFabId;
        var authenticationContext = result.AuthenticationContext;
        Debug.Log("PlayFab login successful. Player ID: " + playFabId);

        // Proceed to load player data or other actions
        PlayFabClientAPI.GetUserData(new GetUserDataRequest{ PlayFabId = playFabId }, OnUserDataSuccess, OnUserDataError);
        
        isLoggedIn = true;
    }

    private void OnUserDataSuccess(GetUserDataResult result)
    {
        Debug.Log("Player data retrieved successfully.");
        isLoggedIn = false;
    }

    private void OnUserDataError(PlayFabError error)
    {
        Debug.LogError("Error retrieving player data: " + error.GenerateErrorReport());
    }

    private void OnLoginError(PlayFabError error)
    {
        Debug.LogError("Error logging into PlayFab: " + error.GenerateErrorReport());
    }

    public void SavePlayerData(string key, string value)
    {

        var request = new UpdateUserDataRequest
        {
            Data = new System.Collections.Generic.Dictionary<string, string>
            {
                { key, value }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataUpdateSuccess, OnDataUpdateError);
    }

    private void OnDataUpdateSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Player data updated successfully.");
    }

    private void OnDataUpdateError(PlayFabError error)
    {
        Debug.LogError("Error updating player data: " + error.GenerateErrorReport());
    }

    public void GetUserData(string playFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = playFabId }, OnGetUserDataSuccess, OnGetUserDataError);
    }

    public void OnGetUserDataSuccess(GetUserDataResult result)
    {
        Debug.Log("User  data retrieved successfully!");
        if (result.Data != null && result.Data.Count > 0)
        {
            foreach (var item in result.Data)
            {
                Debug.Log($"Key: {item.Key}, Value: {item.Value.Value}");
                playerData.Add(item.Key, item.Value.Value);
            }
        }
        else
        {
            Debug.Log("No user data found.");
        }
    }

    public void OnGetUserDataError(PlayFabError error)
    {
        Debug.LogError("Error retrieving user data: " + error.ErrorMessage);
    }
}
