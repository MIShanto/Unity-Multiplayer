using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RTSHealthDisplay : MonoBehaviour
{
    [SerializeField] RTSHealth rTSHealth;
    [SerializeField] GameObject healthBarParent;
    [SerializeField] Image healthBarUI;

    private void Awake()
    {
        rTSHealth.clientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDestroy()
    {
        rTSHealth.clientOnHealthUpdated -= HandleHealthUpdated;
    }

    void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarUI.fillAmount = (float)currentHealth / maxHealth;
    }

    private void OnMouseEnter()
    {
        healthBarParent.SetActive(true);
    }
    private void OnMouseExit()
    {
        healthBarParent.SetActive(false);
    }


}
