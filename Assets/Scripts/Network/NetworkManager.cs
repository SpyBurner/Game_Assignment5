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
    private GameObject photonStatus;

    public TMP_Text userNickNameText;

    public TMP_Text accountText;

    public TMP_Text passwordText;

    void Start()
    {
        PhotonNetwork.OfflineMode = false;
        photonStatus = GameObject.Find("PhotonStatus");

        if (userNickNameText == null)
        {
            throw new System.Exception("Username text is not set");
        }
        if (accountText == null)
        {
            throw new System.Exception("Account text is not set");
        }
        if (passwordText == null)
        {
            throw new System.Exception("Password text is not set");
        }
    }

    void Update() {
        if (photonStatus != null)
        {
            photonStatus.GetComponent<TMP_Text>().text = PhotonNetwork.NetworkClientState.ToString();
        }

        if (userNickNameText != null)
        {
            userNickNameText.text = PhotonNetwork.NickName;
        }
    }

    public void Login() {
        string username = userNickNameText.text;

        if (string.IsNullOrEmpty(username))
        {
            Debug.Log("Username is empty");
            return;
        }

        PhotonNetwork.NickName = username;

        PhotonNetwork.ConnectUsingSettings();
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