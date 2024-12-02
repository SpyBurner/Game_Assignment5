using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : PhotonSingleton<NetworkManager>
{
    public bool isConnected = false;
    
    void Start()
    {

    }

    void Update() 
    {

    }

#region PhotonConenction
    public void ConnectToPun() {
        if (!PlayFabManager.Instance.isLoggedIn) 
        {
            Debug.Log("Not logged in");
            return;
        }

        string username = PlayFabManager.Instance.playerData["displayName"];

        if (string.IsNullOrEmpty(username))
        {
            Debug.Log("Username is empty");
            return;
        }

        PhotonNetwork.NickName = username;

        PhotonNetwork.ConnectUsingSettings();
        isConnected = true;
    }

    public void DisconnectPun() {
        PhotonNetwork.Disconnect();
        isConnected = false;
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
#endregion


#region Chat

#endregion


#region PlayFab Authentication
    public void LoginWithPlayFab() 
    {

    }
#endregion
}