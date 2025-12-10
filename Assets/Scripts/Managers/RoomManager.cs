using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : NetworkBehaviour
{

    [SyncVar] public int playerCount;
    public CustomNetworkRoomManager networkRoomManager;

    public CustomNetworkRoomPlayer roomPlayer;

    [SerializeField] private Button readyButton;
    [SerializeField] private TMP_Text readyButtonText;
    [SerializeField] private Button leaveButton;

    [SerializeField] private string readyButtonTextEnabled = "Ready";
    [SerializeField] private string readyButtonTextDisabled = "Waiting";

    private void Awake()
    {
        RoomManager[] objs = FindObjectsByType<RoomManager>(FindObjectsSortMode.None);

        if (objs.Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        networkRoomManager = FindFirstObjectByType<CustomNetworkRoomManager>();
        networkRoomManager.roomManager = this;
    }

    public void RoomSceneStart()
    {
        if (readyButton == null)
            readyButton = GameObject.Find("Ready Button").GetComponent<Button>();
        readyButton.onClick.AddListener(ReadyButton);
        if (readyButtonText == null)
            readyButtonText = GameObject.Find("Ready Button Text").GetComponent<TMP_Text>();
        Debug.Log(playerCount);
        readyButtonText.text = playerCount == 2 ? readyButtonTextDisabled : readyButtonTextEnabled;
        if (leaveButton == null)
            leaveButton = GameObject.Find("Leave Button").GetComponent<Button>();
        leaveButton.onClick.AddListener(LeaveButton);

        readyButton.interactable = playerCount == 2;
        leaveButton.interactable = true;
    }

    public void GameSceneStart()
    {
        leaveButton = null;
        readyButton = null;
    }

    public void OnPlayerConnect()
    {
        if (isServer)
            playerCount++;

        if (playerCount == 2)
        {
            readyButton.interactable = true;
            readyButtonText.text = readyButtonTextEnabled;
        }
        else
        {
            readyButton.interactable = false;
            readyButtonText.text = readyButtonTextDisabled;
        }
    }

    public void OnPlayerDisconnect()
    {
        if (isLocalPlayer)
        {
            Destroy(gameObject);
            return;
        }

        if (networkRoomManager.allPlayersReady) return;

        if (isServer)
            playerCount--;

        if (roomPlayer.readyToBegin)
        {
            roomPlayer.CmdChangeReadyState(false);
            readyButtonText.text = "Cancel";
        }

        readyButton.interactable = false;
        readyButtonText.text = readyButtonTextDisabled;
    }

    public void ReadyButton()
    {
        roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
        readyButtonText.text = !roomPlayer.readyToBegin ? "Cancel" : "Ready";
        leaveButton.interactable = roomPlayer.readyToBegin;
    }

    public void LeaveButton()
    {
        if (isServer)
            networkRoomManager.StopHost();
        else
            networkRoomManager.StopClient();
    }
}
