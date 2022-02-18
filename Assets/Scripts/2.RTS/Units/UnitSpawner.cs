using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;
public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject unitPrefab;
    [SerializeField] Transform unitSpawnPoint;
    [SerializeField] RTSHealth health;



    #region server

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    void ServerHandleDie()
    {
       // NetworkServer.Destroy(gameObject);
    }

    [Command]
    void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(unitPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion

    #region client

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        if (!hasAuthority) return;

        CmdSpawnUnit();
    }

    #endregion
}
