using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NekromancerUIController : MonoBehaviour
{
    [SerializeField] private Nekromancer nekromancer = null;
    [SerializeField] private Slider healthBar = null;

    private void Update()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (!nekromancer || !healthBar) return;

        healthBar.value = (float)nekromancer.CurrentHealth / nekromancer.MaxHealth;
    }
}
