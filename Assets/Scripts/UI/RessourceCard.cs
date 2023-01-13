using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RessourceCard : MonoBehaviour
{
    [SerializeField] private Image icon = null;
    [SerializeField] private TMP_Text amount = null;

    public void Init(CollectedRessource _ressource)
    {
        icon.sprite = _ressource.ressource.icon;
        amount.text = _ressource.amount.ToString();
    }
}
