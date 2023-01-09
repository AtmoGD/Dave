using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private UIMenuController pauseUI = null;

    public void Pause(bool _pause)
    {
        pauseUI.SetIsActive(_pause);
    }
}
