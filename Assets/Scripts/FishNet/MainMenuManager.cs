using System;
using System.Collections;
using FishNet.Managing;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    [SerializeField] private string menuSceneName;
    [SerializeField] private string gameSceneName;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private TMP_InputField lobbyInput;
    [SerializeField] private TMP_Text lobbyTitle;
    [SerializeField] private TMP_Text lobbyIDText;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button tugboatStartGameButton;

    private void Awake() => instance = this;

    private void Start()
    {
        Client client = FindAnyObjectByType<Client>();

        if (Settings.isUsingSteamworks && client != null)
        {
            NetworkManager networkManager = FindFirstObjectByType<NetworkManager>();
            LobbyEntered(SteamMatchmaking.GetLobbyData(new CSteamID(BootstrapManager.currentLobbyID), "name"), networkManager.IsServerStarted);
        }
        else
        {
            OpenMainMenu();

            if (Settings.isUsingSteamworks) return;

            tugboatStartGameButton.gameObject.SetActive(true);

            if (client != null)
                client.Unload();
        }
    }

    public void CreateLobby()
    {
        menuCanvas.SetActive(false);
        BootstrapManager.CreateLobby();
    }

    public void OpenMainMenu()
    {
        CloseAllScreens();
        menuCanvas.SetActive(true);
    }

    public void OpenLobbyMenu()
    {
        CloseAllScreens();
        lobbyCanvas.SetActive(true);
    }

    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        instance.lobbyTitle.text = lobbyName;
        instance.startGameButton.gameObject.SetActive(isHost);
        instance.lobbyIDText.text = BootstrapManager.currentLobbyID.ToString();
        instance.OpenLobbyMenu();
    }

    private void CloseAllScreens()
    {
        menuCanvas.SetActive(false);
        lobbyCanvas.SetActive(false);
    }

    public void JoinLobby()
    {
        menuCanvas.SetActive(false);
        CSteamID steamID = new(Convert.ToUInt64(lobbyInput.text));
        BootstrapManager.JoinByID(steamID);
    }

    public void LeaveLobby()
    {
        BootstrapManager.LeaveLobby();

        SceneManager.UnloadSceneAsync(menuSceneName);
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Additive);
    }

    public void StartGame()
    {
        string[] scenesToClose = {menuSceneName};
        BootstrapNetworkManager.ChangeNetworkScene(gameSceneName, scenesToClose);
    }
}
