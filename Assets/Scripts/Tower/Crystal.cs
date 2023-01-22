using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Crystal : AttackTower
{
    public Action OnCrystalDestroyed;

    public override void Start()
    {
        base.Start();

        LevelManager.SetCrystal(this);
    }

    public override void Die()
    {
        OnCrystalDestroyed?.Invoke();

        print("Crystal destroyed -> WTF???????????????????????????????");

        base.Die();
    }

    public override void Interact(Nekromancer _nekromancer)
    {
        base.Interact(_nekromancer);

        if (LevelManager.Instance.CrystalFull)
        {
            GameManager.Instance.PlayerController.OpenEndGameMenu();
        }
    }
}
