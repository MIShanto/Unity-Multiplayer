using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] float destroyAfterSec = 3f;
    [SerializeField] float launchForce = 10f;
    [SerializeField] int damageToDeal = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfterSec);
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkIdentity networkIdentity))
        {
            if (networkIdentity.connectionToClient == connectionToClient) return;
        }

        if (other.TryGetComponent(out RTSHealth rTSHealth))
        {
            rTSHealth.DealDamage(damageToDeal);

            DestroySelf();
        }
    }
}
