using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    private IDamagable target = null;
    [SerializeField] private Canvas healthCanvas = null;
    [SerializeField] private Slider healthBar = null;

    private void OnEnable()
    {
        target = GetComponentInParent<IDamagable>();
        if (target == null)
        {
            Debug.LogError("No IDamagable component found on parent of HealthbarController");
            return;
        }

        healthCanvas.worldCamera = Camera.main;
        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = (float)target.Health / target.MaxHealth;

        healthCanvas.enabled = healthBar.value < 1f;
    }
}
