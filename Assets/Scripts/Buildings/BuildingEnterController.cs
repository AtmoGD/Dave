using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEnterController : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent onEnter = null;
    [SerializeField] private ParticleSystem particles = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Nekromancer nekromancer = other.GetComponent<Nekromancer>();
        if (nekromancer)
        {
            onEnter?.Invoke();
        }
    }

    public void OpenRitualMenu()
    {
        Nekromancer nekromancer = FindObjectOfType<Nekromancer>();
        if (nekromancer)
        {
            nekromancer.PlayerController.OpenRitualMenu();
            nekromancer.StopMovement();
            if (particles)
                particles.Play();
        }
    }

    public void OpenLevelMenu()
    {
        Nekromancer nekromancer = FindObjectOfType<Nekromancer>();
        if (nekromancer)
        {
            nekromancer.PlayerController.OpenChooseLevelMenu();
            nekromancer.StopMovement();
            if (particles)
                particles.Play();
        }
    }
}
