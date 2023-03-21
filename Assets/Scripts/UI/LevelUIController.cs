using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text collectedSoulsText = null;
    [SerializeField] private Ressource soulRessource = null;
    private CollectedRessource soul = null;
    private void Update()
    {
        if (!collectedSoulsText) return;

        soul = LevelManager.Instance.GatheredRessources.Find(r => r.ressource == soulRessource);
        if (soul != null)
        {
            collectedSoulsText.text = (Mathf.FloorToInt(soul.amount * GameManager.Instance.PlayerController.Nekromancer.SoulMultiplikator)).ToString();
        }
        else
        {
            collectedSoulsText.text = "0";
        }
    }
}
