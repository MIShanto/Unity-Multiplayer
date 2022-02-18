using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSHealth : NetworkBehaviour
{

    [SerializeField] int maxHealth = 100;

    [SyncVar(hook =nameof(HandleHealthUpdated))]
    [SerializeField]int currentHealth;

    public event Action ServerOnDie;
    public event Action<int, int> clientOnHealthUpdated; // <int, int> = <newHealth, Maxhealth>

    #region server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(int damageValue)
    {

        if (currentHealth == 0) return;

        currentHealth = Mathf.Max(currentHealth - damageValue, 0);

        if (currentHealth != 0) return;

        ServerOnDie?.Invoke();

        Debug.LogError(87879878);
    }

    #endregion

    #region client

    void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        clientOnHealthUpdated?.Invoke(newHealth,maxHealth);
    }

    #endregion
}
