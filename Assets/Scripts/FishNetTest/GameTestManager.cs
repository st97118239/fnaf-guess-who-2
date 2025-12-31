using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class GameTestManager : NetworkBehaviour
{
    public readonly SyncVar<string> syncedString = new SyncVar<string>();

    private void Start()
    {
        if (IsServerStarted)
            syncedString.Value = "aaa";

        Debug.Log(syncedString.Value);
    }
}
