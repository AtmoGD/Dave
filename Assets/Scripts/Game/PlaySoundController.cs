using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundController : MonoBehaviour
{
    [SerializeField] private FMODUnity.StudioEventEmitter sound = null;

    public void PlaySound()
    {
        sound?.Play();
    }
}
