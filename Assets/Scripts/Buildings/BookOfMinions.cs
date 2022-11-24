using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOfMinions : Building
{
    [SerializeField] MinionData minion = null;
    [SerializeField] GameObject spawnMinionUI = null;

    public override void Interact(Nekromancer _nekromancer)
    {
        base.Interact(_nekromancer);

        spawnMinionUI.SetActive(true);
    }

    public void SpawnMinion()
    {
        if (minion)
        {
            GameObject minionInstance = Instantiate(minion.prefab, transform.position, Quaternion.identity);
            spawnMinionUI.SetActive(false);
        }
    }
}
