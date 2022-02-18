using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] Targeter targeter;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;

    [SerializeField] float fireRange = 5f; // should be grater than chase range.
    [SerializeField] float fireRate = 1f;
    [SerializeField] float rotationSpeed = 20f;

    float lastFireTime;

    [ServerCallback]

    private void Update()
    {
        if (targeter.GetTarget() == null) return;

        if (!CanFireAtTarget()) return;

        Quaternion targetRotation = Quaternion.LookRotation(targeter.GetTarget().transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / fireRate) + lastFireTime)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(targeter.GetTarget().GetAimPoint().position - projectileSpawnPoint.position);

            GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            lastFireTime = Time.time;
        }
    }

    [Server]
    bool CanFireAtTarget()
    {
        return ((targeter.GetTarget().transform.position - transform.position).sqrMagnitude <= fireRange * fireRange);
    }

}
