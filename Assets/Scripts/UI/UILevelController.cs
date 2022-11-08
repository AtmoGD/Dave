using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILevelController : MonoBehaviour
{
    [SerializeField] private LevelManager gameManager = null;

    [SerializeField] private TMP_Text cycleState = null;

    private void Awake()
    {
        gameManager.OnCycleChanged += UpdateCycleState;
    }

    private void UpdateCycleState(CycleState _cycleState)
    {
        if (!cycleState) return;

        this.cycleState.text = _cycleState.Cycle.ToString();
    }

    public void SpawnEnemy()
    {

    }
}
