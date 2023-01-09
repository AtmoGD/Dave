using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILevelController : MonoBehaviour
{
    [SerializeField] private TMP_Text collectedSoulsText = null;
    [SerializeField] private Ressource soulRessource = null;
    private void Update()
    {
        if (!collectedSoulsText) return;

        collectedSoulsText.text = LevelManager.Instance.GatheredRessources.Count.ToString();

    }
}
