using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;

public class ChatManager : PersistentSingleton<ChatManager>, IChatClientListener
{
    public ChatClient chatClient;
    public bool isConnected = false;
    public string userId;

#region IChatClientListener
    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
#endregion

    void Start()
    {
        chatClient = new ChatClient(this);
        if (!PlayFabManager.Instance.isLoggedIn || NetworkManager.Instance.isConnected)
        {
            Debug.Log("Not connected to network yet.");
            return;
        }

        userId = PlayFabManager.Instance.playFabId;
    }

    void Update()
    {
        if (isConnected) chatClient.Service();
    }

    public void ConnectChat()
    {
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userId));
        isConnected = true;
    }
}
