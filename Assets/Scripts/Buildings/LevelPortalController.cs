using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Nekromancer nekromancer = other.GetComponent<Nekromancer>();
        if (nekromancer)
        {
            nekromancer.PlayerController.OpenChooseLevelMenu();
        }
    }
}
