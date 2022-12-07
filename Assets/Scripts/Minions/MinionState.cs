using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionState
{
    protected Minion minion;

    public virtual void Enter(Minion _minion)
    {
        minion = _minion;
    }

    public virtual void FrameUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
