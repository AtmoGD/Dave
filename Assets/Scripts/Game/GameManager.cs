using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

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
    [SerializeField] private float nightMusicOffset = 4f;
    [SerializeField] private float menuMusicOffset = 4f;
    [SerializeField] private FMODUnity.StudioEventEmitter levelMusic = null;
    [SerializeField] private FMODUnity.StudioEventEmitter campMusic = null;
    [SerializeField] private FMODUnity.StudioEventEmitter menuMusic = null;
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

    [field: SerializeField] public bool IsPaused { get; set; } = false;
    [field: SerializeField] public float TimeInGame { get; private set; } = 0f;

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

    private void Update()
    {
        TimeInGame += Time.deltaTime;

        UpdateMusic();
    }

    public void ChangeGameState(GameState _state)
    {
        WorldGrid.Instance.DeleteAllChildren();
        gameState = _state;

        // if (gameState == GameState.MainMenu)
        //     PlayerController.UIController.OpenMenu(PlayerController.UIController.titlescreenUI);
    }

    public void UpdateMusic()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Game_Paused", IsPaused ? 1 : 0);

        switch (GameState)
        {
            case GameState.MainMenu:
                if (TimeInGame < menuMusicOffset) break;

                if (levelMusic.IsPlaying())
                    levelMusic.Stop();
                if (campMusic.IsPlaying())
                    campMusic.Stop();
                if (!menuMusic.IsPlaying())
                    menuMusic.Play();
                break;
            case GameState.Camp:
                if (levelMusic.IsPlaying())
                    levelMusic.Stop();
                if (!campMusic.IsPlaying())
                    campMusic.Play();
                if (menuMusic.IsPlaying())
                    menuMusic.Stop();
                break;
            case GameState.Level:
                if (campMusic.IsPlaying())
                    campMusic.Stop();
                if (!levelMusic.IsPlaying())
                    levelMusic.Play();
                if (menuMusic.IsPlaying())
                    menuMusic.Stop();

                if (LevelManager.Instance.CurrentCycleState.Cycle == Cycle.Day &&
                LevelManager.Instance.CurrentCycleState.TimeLeft > nightMusicOffset)
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Daily-cycle", 0f);
                else
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Daily-cycle", 1f);


                break;
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
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
