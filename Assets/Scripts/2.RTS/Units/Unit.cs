using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class Unit : NetworkBehaviour
{
    [SerializeField] UnityEvent onSelected; 
    [SerializeField] UnityEvent onDeSelected;
    [SerializeField] UnitMovement unitMovement;

    public static event Action<Unit> ServerOnPlayerSpawn;
    public static event Action<Unit> ServerOnPlayerDeSpawn;

    public static event Action<Unit> AuthorityOnPlayerSpawn;
    public static event Action<Unit> AuthorityOnPlayerDeSpawn;

    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }

    #region server

    public override void OnStartServer()
    {
        ServerOnPlayerSpawn?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnPlayerDeSpawn?.Invoke(this);
    }

    #endregion

    #region client

    public override void OnStartClient()
    {
        if (!hasAuthority || !isClientOnly) return;

        AuthorityOnPlayerSpawn?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!hasAuthority || !isClientOnly) return;

        AuthorityOnPlayerDeSpawn?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if (!hasAuthority) return;

        onSelected?.Invoke();
    }

    [Client]
    public void DeSelect()
    {
        if (!hasAuthority) return;

        onDeSelected?.Invoke();
    }

    #endregion
}
