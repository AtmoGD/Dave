using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class StopMusic : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter musicEmitter = null;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         musicEmitter.Stop();
    //     }
    // }

    public void Stop()
    {
        if (musicEmitter != null && musicEmitter.IsPlaying())
            musicEmitter.Stop();
    }
}
