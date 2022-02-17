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

        TryMove(hit.point);
        
    }

    private void TryMove(Vector3 point)
    {
        foreach (Unit player in unitComandGiver.selectedUnits) // move only selected players
        {
            player.GetUnitMovement().CmdUnitMove(point);
        }
    }

    #endregion
}
