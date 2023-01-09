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
    [SerializeField] private GameState gameState = GameState.Level;
    [SerializeField] private GameUIController gameUIController = null;
    [SerializeField] private PlayerController playerController = null;
    public PlayerController PlayerController { get { return playerController; } }
    [SerializeField] private DataList dataList = null;
    public DataList DataList { get { return dataList; } }

    [SerializeField] private WorldGrid worldGrid = null;
    public WorldGrid WorldGrid { get { return worldGrid; } }

    [SerializeField] public bool startOnLoad = true;
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
    }

    protected void Start()
    {
        if (startOnLoad) StartGame();
    }

    public void Update()
    {

    }

    private void StartGame()
    {
        playerController.Init();
    }

    public void PauseGame(InputAction.CallbackContext _context)
    {
        print("Pause Game");

        if (_context.performed)
        {
            PauseGame(!IsPaused);
        }
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            IsPaused = true;
            LevelManager.Instance.StopTime();
            gameUIController.Pause(true);
        }
        else
        {
            IsPaused = false;
            LevelManager.Instance.StartTime();
            gameUIController.Pause(false);
        }
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
