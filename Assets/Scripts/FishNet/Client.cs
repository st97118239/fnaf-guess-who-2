using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;

public class Client : NetworkBehaviour
{
    public bool isLocalPlayer;
    public bool isOpponent;

    public readonly SyncVar<int> player = new();

    public void Load()
    {
        if (!IsOwner) return;

        int playerId = IsServerStarted ? 1 : 2;
        SetPlayer(playerId);

        Debug.Log("Loaded player " + playerId);

        foreach (Canvas canv in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
        {
            if (canv.gameObject.name != "MainCanvas") continue;
            canv.transform.GetChild(0).GetComponent<TMP_Text>().text = playerId.ToString();
            break;
        }
    }

    public void Unload()
    {
        isLocalPlayer = false;
        isOpponent = false;
        SetPlayer(0);
    }

    [ServerRpc] private void SetPlayer(int value) => player.Value = value;
}
