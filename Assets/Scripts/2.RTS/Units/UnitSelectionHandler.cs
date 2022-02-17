using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    Camera cam;
    [SerializeField] LayerMask layerMask;

    public List<Unit> selectedUnits { get; } = new List<Unit>();
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            foreach (Unit selectedPlayer in selectedUnits)
            {
                selectedPlayer.DeSelect();
            }

            selectedUnits.Clear();
        }

        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
    }

    private void ClearSelectionArea()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        if (!hit.collider.TryGetComponent(out Unit unit)) return;

        if (!unit.hasAuthority) return;

        selectedUnits.Add(unit);

        foreach (Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Select();
        }
    }
}
