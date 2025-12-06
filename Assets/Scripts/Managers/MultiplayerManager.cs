using System.Linq;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public CustomNetworkRoomPlayer[] roomPlayers;
    public Player[] players;

    private void Start()
    {
        roomPlayers = FindObjectsByType<CustomNetworkRoomPlayer>(FindObjectsSortMode.InstanceID);

        roomPlayers = roomPlayers.OrderBy(player => player.index).ToArray();

        players = FindObjectsByType<Player>(FindObjectsSortMode.None);

        players = players.OrderBy(player => player.playerIdx).ToArray();
    }
}
