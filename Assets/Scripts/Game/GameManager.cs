using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }
    [SerializeField] private PlayerController playerController = null;
    public PlayerController PlayerController { get { return playerController; } }
    [SerializeField] private InputController inputController = null;
    public InputController InputController { get { return inputController; } }
    [SerializeField] private float timeScale = 1f;
    [SerializeField] private WorldGrid worldGrid = null;
    public WorldGrid WorldGrid { get { return worldGrid; } }
    public float TimeScale { get { return timeScale; } }

    [SerializeField] public bool startOnLoad = true;

    GridElement lastHoveredGridElement = null;

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
        playerController.Init(this, inputController);
    }
}
