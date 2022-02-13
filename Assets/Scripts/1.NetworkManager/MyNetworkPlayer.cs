using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar] [SerializeField] string displayName = "Missing name"; // default name
    [SyncVar(hook = nameof(HandleColor))] [SerializeField] Color displayColor = Color.white; // default color

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

    public void HandleColor(Color oldColor, Color newColor) // this method is called when display color variable is updated as it is hooked.. 
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", newColor);

        //Debug.LogError(oldColor);
        //Debug.LogError(newColor);
    }
}
