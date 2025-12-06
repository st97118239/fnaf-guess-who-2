using System;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] public int playerIdx;

    public CustomNetworkRoomPlayer roomPlayer;

    private NetworkIdentity networkIdentity;

    private void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();

        if (isServer)
        {
            playerIdx = Convert.ToInt32(roomPlayer.GetComponent<NetworkIdentity>().netId);
            playerIdx--;
        }
    }
}
