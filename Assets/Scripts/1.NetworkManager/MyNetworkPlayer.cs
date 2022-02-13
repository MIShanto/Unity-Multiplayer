using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleName))] [SerializeField] string displayName = "Missing name"; // default name
    [SyncVar(hook = nameof(HandleColor))] [SerializeField] Color displayColor = Color.white; // default color

    [SerializeField] Renderer renderer;
    [SerializeField] TextMeshProUGUI nameTextUI;

    #region server
    [Server]
    public void SetDisplayName(string newDisplayName) 
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newdisplayColor)
    {
        displayColor = newdisplayColor;
        
    }

    [Command]
    void CmdSetDisplayName(string newDisplayName) // updates name when get request from client..
    {
        RpcLogNewName(newDisplayName); // server calling all client.
        
        SetDisplayName(newDisplayName);

    }
    #endregion

    #region client
    public void HandleColor(Color oldColor, Color newColor) // this method is called when display color variable is updated as it is hooked.. 
    {
        renderer.material.SetColor("_BaseColor", newColor);

        //Debug.LogError(oldColor);
        //Debug.LogError(newColor);
    }

    public void HandleName(string oldName, string newName) // this method is called when display name variable is updated as it is hooked.. 
    {
        nameTextUI.text = newName;

    }

    [ContextMenu("Set new name")]
    void UpdateDisplayName() // client asks server to update name..
    {
        CmdSetDisplayName("Name updated.");
    }

    [ClientRpc]
    void RpcLogNewName(string newDisplayName) // when server calls all client..
    {
        Debug.LogError(newDisplayName);
    }

    #endregion
}
