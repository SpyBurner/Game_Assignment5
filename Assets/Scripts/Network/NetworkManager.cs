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
    public TMP_Text accountText;
    public TMP_Text passwordText;

    void Start()
    {
        PhotonNetwork.OfflineMode = false;
        photonStatus = GameObject.Find("PhotonStatus");
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
    }

    public void Login() {
        string username = PlayFabManager.Instance.playerData["displayName"];

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

    #region PlayFab Authentication
    public void LoginWithPlayFab() {
        PlayFabManager.Instance.LoginWithEmailAndPassword(accountText.text, passwordText.text);

        if (PlayFabManager.Instance.isLoggedIn) {
            Login();
        }
    }
    #endregion
}