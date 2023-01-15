using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSouls : MonoBehaviour
{
    [SerializeField] private Collectable collectable = null;
    [SerializeField] private TextMeshProUGUI soulsText = null;
    private PlayerController playerController = null;

    private void Start()
    {
        playerController = GameManager.Instance.PlayerController;

        if (!playerController || !soulsText) return;

        playerController.OnSoulsChanged += UpdateText;

        UpdateText();
    }

    private void OnDestroy()
    {
        if (!playerController) return;

        playerController.OnSoulsChanged -= UpdateText;
    }

    private void UpdateText()
    {
        if (!playerController || !soulsText) return;

        soulsText.text = playerController.PlayerData.collectables.FindAll(x => x == collectable.id).Count.ToString();
    }
}
