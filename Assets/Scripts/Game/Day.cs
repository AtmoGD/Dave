using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Day : CycleState
{
    [SerializeField] private List<PortalWave> waves = new List<PortalWave>();
    public override void Enter(LevelManager _gameManager)
    {
        base.Enter(_gameManager);

        _gameManager.InputController.ChangeActionMap("Build");
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
