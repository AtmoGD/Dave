using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PerkVisualizer : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private GameObject perkVisualizer = null;
    [SerializeField] private TMPro.TMP_Text perkPointsText = null;

    public string CurrentPerkControll
    {
        get
        {
            if (playerController.PlayerInput.currentControlScheme == "Controller")
                return "X";
            else
                return "T";
        }
    }

    private void Update()
    {
        perkVisualizer.SetActive(playerController.PerkPoints > 0 && GameManager.Instance.GameState == GameState.Level);
        perkPointsText.text = CurrentPerkControll;
    }
}
