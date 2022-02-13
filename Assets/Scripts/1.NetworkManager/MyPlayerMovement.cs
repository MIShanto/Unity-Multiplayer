using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyPlayerMovement : NetworkBehaviour
{
    Camera cam;

    [Command]
    void CmdPlayerMove(Vector3 destinationPos)
    {
        transform.position = destinationPos;
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        cam = Camera.main;
    }

    [ClientCallback]
    void Update() // thus update is only called in client slide..
    {
        if (!Input.GetMouseButtonDown(0)) return;

        RaycastHit hit;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.LogError(hit);

            if(hit.collider.CompareTag("c"))
                CmdPlayerMove(hit.point);
        }
    }
}
