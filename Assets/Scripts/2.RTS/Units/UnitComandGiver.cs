using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitComandGiver : MonoBehaviour
{
    Camera cam;
    [SerializeField] UnitSelectionHandler unitComandGiver;
    [SerializeField] LayerMask layerMask;
    private void Start()
    {
        cam = Camera.main;
    }

    #region client

    void Update() 
    {
        
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        RaycastHit hit;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return;

        }

        if (hit.collider.TryGetComponent(out Targetable target))
        {
            if (target.hasAuthority)
            {
                TryMove(hit.point);
                return;
            }

            TryTarget(target);
            return;
        }

        TryMove(hit.point);
        
    }

    private void TryMove(Vector3 point)
    {
        foreach (Unit unit in unitComandGiver.selectedUnits) // move only selected players
        {
            unit.GetUnitMovement().CmdUnitMove(point);
        }
    }
    private void TryTarget(Targetable target)
    {
        foreach (Unit unit in unitComandGiver.selectedUnits) // move only selected players
        {
            unit.GetTargeter().CmdSetTarget(target.gameObject);
        }
    }

    #endregion
}
