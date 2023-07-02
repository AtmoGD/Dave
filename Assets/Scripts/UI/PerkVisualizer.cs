using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMODUnity;

public class PerkVisualizer : MonoBehaviour
{
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private GameObject perkVisualizer = null;
    [SerializeField] private TMPro.TMP_Text perkPointsText = null;
    [SerializeField] private FMODUnity.StudioEventEmitter perkSound = null;

    bool isShowing = false;

    public string CurrentPerkControll
    {
        get
        {
            if (playerController.PlayerInput.currentControlScheme == "Controller")
                return "Y";
            else
                return "T";
        }
    }

    private void Update()
    {
        if (playerController.PerkPoints > 0 && GameManager.Instance.GameState == GameState.Level)
        {
            if (!isShowing)
            {
                isShowing = true;
                perkVisualizer.SetActive(true);

                if (!perkSound.IsPlaying())
                    perkSound.Play();
            }
        }
        else
        {
            isShowing = false;
            perkVisualizer.SetActive(false);
        }

        // perkVisualizer.SetActive(playerController.PerkPoints > 0 && GameManager.Instance.GameState == GameState.Level);
        perkPointsText.text = CurrentPerkControll;
    }
}
