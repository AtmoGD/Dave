using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    MainMenu,
    Camp,
    Level
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }
    [Header("Game Manager")]
    [SerializeField] private bool dontDestroyOnLoad = true;
    [SerializeField] private GameState gameState = GameState.Level;
    public GameState GameState { get { return gameState; } }
    [SerializeField] private GameUIController gameUIController = null;
    [SerializeField] private PlayerController playerController = null;
    public Vector2 SpawnPosition
    {
        get
        {
            if (gameState == GameState.Camp)
            {
                return CampManager.Instance.NekromancerSpawnPosition;
            }
            else
            {
                return LevelManager.Instance.LevelData.NekromancerSpawnPosition;
            }
        }
    }
    public PlayerController PlayerController
    {
        get
        {
            if (playerController == null) playerController = FindObjectOfType<PlayerController>();
            return playerController;
        }
    }
    [SerializeField] private DataList dataList = null;
    public DataList DataList { get { return dataList; } }

    [Space(20)]

    GridElement lastHoveredGridElement = null;

    [field: SerializeField] public bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;

        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
    }

    public void ChangeGameState(GameState _state)
    {
        WorldGrid.Instance.DeleteAllChildren();
        gameState = _state;
    }

    public void ReloadScene(InputAction.CallbackContext _context)
    {
        print("Reload Scene");

        if (_context.performed)
        {
            ReloadGame();
        }
    }

    public void ReloadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
