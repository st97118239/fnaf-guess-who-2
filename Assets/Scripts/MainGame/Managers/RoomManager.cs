using FishNet.Object;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private void Start()
    {
        foreach (Client client in FindObjectsByType<Client>(FindObjectsSortMode.None))
        {
            if (client.GetComponent<NetworkObject>().IsOwner)
            {
                client.isLocalPlayer = true;
                client.Load();
            }
            else
                client.isOpponent = true;
        }
    }

    public void Return()
    {
        string[] scenesToClose = { "GameScene" };
        BootstrapNetworkManager.ChangeNetworkScene("MenuScene", scenesToClose);
    }
}
