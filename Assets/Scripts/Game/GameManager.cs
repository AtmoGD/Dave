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

    [SerializeField] public bool startOnLoad = true;

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

    private void StartGame()
    {
        playerController.Init(inputController);
    }
}
