using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public abstract class State { //Preguntar a Pablo

    public readonly EnableTurnEvent ev_enableturn = new EnableTurnEvent();

    public enum turnstate
    {
        turninfo = 0,
        chooseaction = 1,
        qte = 2,
        anim = 3,
    }
    public abstract void InitState();

    public abstract void UpdateState(float delta);

    public abstract void ExitState();
}
