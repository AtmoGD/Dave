using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text collectedSoulsText = null;
    [SerializeField] private Ressource soulRessource = null;
    [SerializeField] private TMP_Text enemyAmountText = null;
    private CollectedRessource soul = null;
    private void Update()
    {
        if (GameManager.Instance.debugging && enemyAmountText)
        {
            if (LevelManager.Instance.CurrentCycleState.Cycle == Cycle.Night)
                enemyAmountText.text = LevelManager.Instance.EnemyCount.ToString();
            else
                enemyAmountText.text = LevelManager.Instance.MinionCount.ToString();
        }
        else
        {
            if (enemyAmountText)
                enemyAmountText.text = "";
        }

        if (!collectedSoulsText) return;

        if (GameManager.Instance.GameState == GameState.Level)
        {
            soul = LevelManager.Instance.GatheredRessources.Find(r => r.ressource == soulRessource);

            if (soul != null)
                collectedSoulsText.text = (Mathf.FloorToInt(soul.amount * GameManager.Instance.PlayerController.Nekromancer.SoulMultiplikator)).ToString();
            else
                collectedSoulsText.text = "0";
        }
        else
        {
            collectedSoulsText.text = GameManager.Instance.PlayerController.PlayerData.collectables.Count.ToString();
        }
    }
}
