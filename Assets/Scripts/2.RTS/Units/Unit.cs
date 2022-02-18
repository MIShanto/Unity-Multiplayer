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
    [SerializeField] Targeter targeter;
    [SerializeField] RTSHealth health;


    public static event Action<Unit> ServerOnPlayerSpawn;
    public static event Action<Unit> ServerOnPlayerDeSpawn;

    public static event Action<Unit> AuthorityOnPlayerSpawn;
    public static event Action<Unit> AuthorityOnPlayerDeSpawn;

    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }
    public Targeter GetTargeter()
    {
        return targeter;
    }

    #region server

    public override void OnStartServer()
    {
        ServerOnPlayerSpawn?.Invoke(this);

        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        ServerOnPlayerDeSpawn?.Invoke(this);

        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region client

    public override void OnStartAuthority()
    {
        if (!isClientOnly) return;

        AuthorityOnPlayerSpawn?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly) return;

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
