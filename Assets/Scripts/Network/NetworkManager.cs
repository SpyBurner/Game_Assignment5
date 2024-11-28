using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Firebase.Auth;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UI;

public class NetworkManager : PhotonSingleton<NetworkManager>
{
    private GameObject photonStatus;

    private FirebaseManager firebaseManager;

    public Text userNameText;

    void Start()
    {
        PhotonNetwork.OfflineMode = false;
        photonStatus = GameObject.Find("PhotonStatus");

        if (firebaseManager == null)
        {
            firebaseManager = FirebaseManager.Instance;
        }

        if (userNameText == null)
        {
            throw new System.Exception("Username text is not set");
        }
    }

    void Update() {
        if (photonStatus != null)
        {
            photonStatus.GetComponent<Text>().text = PhotonNetwork.NetworkClientState.ToString();
        }

        if (userNameText != null)
        {
            userNameText.text = PhotonNetwork.NickName;
        }
    }

    public void LoginWithFirebase(string email, string password) {
        if (firebaseManager.GetCurrentUser() != null)
        {
            Debug.Log("User is already logged in.");
            return;
        }

        firebaseManager.Login(email, password, (result) => {
            if (!result)
            {
                Debug.LogError("Login with Firebase was failed.");
            }
            else {
                PhotonNetwork.ConnectUsingSettings();
            }
        });
    }

    public void Login() {
        string username = userNameText.text;

        if (string.IsNullOrEmpty(username))
        {
            Debug.Log("Username is empty");
            return;
        }

        PhotonNetwork.NickName = username;

        PhotonNetwork.ConnectUsingSettings();
    }

    public void LogoutWithFirebase() {
        if (firebaseManager.GetCurrentUser() == null)
        {
            Debug.Log("User is not logged in.");
            return;
        }

        firebaseManager.Logout();
        PhotonNetwork.Disconnect();
    }

    public void Logout() {
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Disconnected from server: " + cause.ToString());
    }
}