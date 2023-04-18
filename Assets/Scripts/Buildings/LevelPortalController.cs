using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortalController : MonoBehaviour
{
    [SerializeField] private ParticleSystem portalParticles = null;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Nekromancer nekromancer = other.GetComponent<Nekromancer>();
        if (nekromancer)
        {
            nekromancer.PlayerController.OpenChooseLevelMenu();
            nekromancer.CurrentInput.MoveDir = Vector2.zero;
            if (portalParticles)
                portalParticles.Play();
        }
    }
}
