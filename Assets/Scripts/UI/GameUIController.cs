using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Animator loadingScreen = null;

    public void StartLoadGame()
    {
        loadingScreen.SetTrigger("StartLoading");
    }

    public void StartLevel()
    {
        GameManager.Instance.ChangeGameState(GameState.Level);
        StartLoadGame();
    }

    public void StartCampLevel()
    {
        GameManager.Instance.ChangeGameState(GameState.Camp);
        StartLoadGame();
    }
}
