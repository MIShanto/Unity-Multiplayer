using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
public class UnitSelectionHandler : MonoBehaviour
{
    Camera cam;
    [SerializeField] LayerMask layerMask;

    [SerializeField] RectTransform dragArea;

    Vector2 dragStartPos, dragStopPos;

    RTSPlayer player;
    public List<Unit> selectedUnits { get; } = new List<Unit>();
    private void Start()
    {
        cam = Camera.main;

        Unit.AuthorityOnPlayerDeSpawn += AuthorityHandleUnitDespawned;
    }

    private void OnDestroy()
    {
        Unit.AuthorityOnPlayerDeSpawn -= AuthorityHandleUnitDespawned;
    }
    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        selectedUnits.Remove(unit);
    }

    private void Update()
    {
        if(player == null)
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }

        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }

        else if (Mouse.current.rightButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (Unit selectedPlayer in selectedUnits)
            {
                selectedPlayer.DeSelect();
            }

            selectedUnits.Clear();
        }
        

        dragArea.gameObject.SetActive(true);

        dragStartPos = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }
    
    private void UpdateSelectionArea()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        float areaWidth = mousePos.x - dragStartPos.x;
        float areaHeight = mousePos.y - dragStartPos.y;

        dragArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));

        dragArea.anchoredPosition = dragStartPos +
            new Vector2(areaWidth / 2 , areaHeight / 2);
    }

    private void ClearSelectionArea()
    {
        dragArea.gameObject.SetActive(false);

        if (dragArea.sizeDelta.magnitude == 0) // if select 1 unit individually
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

            return;
        }

        //calc the bonding box (in screen position)
        Vector2 min = dragArea.anchoredPosition - (dragArea.sizeDelta / 2);
        Vector2 max = dragArea.anchoredPosition + (dragArea.sizeDelta / 2);

        //check for units inside the area
        foreach (Unit unit in player.GetUnitList())
        {
            if (selectedUnits.Contains(unit)) continue;

            //get screen position of units. because they are in world pos.
            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x &&
                screenPos.y > min.y && screenPos.y < max.y)
            {
                selectedUnits.Add(unit);

                unit.Select();
            }
        }
    }
}
