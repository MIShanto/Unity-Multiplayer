using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] List<Unit> myUnits = new List<Unit>();

    #region server
    public override void OnStartServer()
    {
        Unit.ServerOnPlayerSpawn += ServerhandleUnitSpawed;
        Unit.ServerOnPlayerDeSpawn += ServerhandleUnitDeSpawed;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnPlayerSpawn -= ServerhandleUnitSpawed;
        Unit.ServerOnPlayerDeSpawn -= ServerhandleUnitDeSpawed;
    }

    void ServerhandleUnitSpawed(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Add(unit);
    }
    void ServerhandleUnitDeSpawed(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Remove(unit);
    }
    #endregion



    #region client
    public override void OnStartClient()
    {
        if (!isClientOnly) return;

        Unit.AuthorityOnPlayerSpawn += AuthorityhandleUnitSpawed;
        Unit.AuthorityOnPlayerDeSpawn += AuthorityhandleUnitDeSpawed;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly) return;

        Unit.AuthorityOnPlayerSpawn -= AuthorityhandleUnitSpawed;
        Unit.AuthorityOnPlayerDeSpawn -= AuthorityhandleUnitDeSpawed;
    }

    void AuthorityhandleUnitSpawed(Unit unit)
    {
        if (!hasAuthority) return;

        myUnits.Add(unit);
    }
    void AuthorityhandleUnitDeSpawed(Unit unit)
    {
        if (!hasAuthority) return;

        myUnits.Remove(unit);
    }

    #endregion
}
