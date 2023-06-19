using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TitleScreenAnimatorController : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter titleScreenMusic = null;

    public void PlayTitleScreenMusic()
    {
        if (GameManager.Instance && GameManager.Instance.GameState == GameState.MainMenu)
            GameManager.Instance.PlayTitleMusic();
    }
}
