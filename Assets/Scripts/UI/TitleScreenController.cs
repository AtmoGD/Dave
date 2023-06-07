using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter titleScreenMusic = null;
    // [SerializeField] private StudioEventEmitter titleScreenAmbience = null;

    private void Awake()
    {
        if (GameManager.Instance == null)
            return;

        // this.gameObject.SetActive(GameManager.Instance.GameState == GameState.MainMenu);
    }

    private void OnEnable()
    {
        titleScreenMusic.Play();
    }
}
