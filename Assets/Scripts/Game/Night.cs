using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Night : CycleState
{
    public override void Enter(LevelManager _gameManager)
    {
        base.Enter(_gameManager);

        _gameManager.InputController.ChangeActionMap("Combat");
    }

    public override void FrameUpdate(float _deltaTime)
    {
        base.FrameUpdate(_deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
