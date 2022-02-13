using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [System.Obsolete]
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log("connected..");

    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        var player = conn.identity.GetComponent<MyNetworkPlayer>();
        Debug.Log("player added..");
        Debug.Log($"no player: {numPlayers}");

        //change name..
        player.SetDisplayName($"player {numPlayers}");

        //change color..
        player.SetDisplayColor(new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }
}
