using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    Camera cam;
    
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Targeter targeter;
    [SerializeField] float chaseRange;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        cam = Camera.main;
    }

    #region server

    [ServerCallback]

    private void Update() // reset path if close to stopping distance..
    {
        Targetable target = targeter.GetTarget();

        if (target != null)
        {
            if ((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange)
                agent.SetDestination(target.transform.position);
            else if (agent.hasPath)
                agent.ResetPath();

            return;
        }
        if (!agent.hasPath) return;

        if (agent.remainingDistance > agent.stoppingDistance) return;

        agent.ResetPath();
    }

    [Command]
    public void CmdUnitMove(Vector3 destinationPos)
    {
        targeter.ClearTarget();

        if (!NavMesh.SamplePosition(destinationPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            return;

        agent.SetDestination(hit.position);
        
    }

    #endregion

    #region client

  /*  [ClientCallback]
    void Update() // thus update is only called in client slide..
    {
        if (!hasAuthority) return;

        *//*as per old input system*//*
        //if (!Input.GetMouseButtonDown(0)) return;

        *//*as per new input system*//*
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        RaycastHit hit;

        *//*as per old input system*//*
        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        *//*as per new input system*//*
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return;
        }

        CmdPlayerMove(hit.point);
    }*/

    #endregion
}
