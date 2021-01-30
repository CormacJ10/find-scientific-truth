using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM : FSM
{

    public override NPC.NPCState PickNextState(NPC.NPCState curStateType)
    {
        NPC.NPCState nextStateType;
        switch (curStateType)
        {
            case NPC.NPCState.Idle:
                nextStateType = NPC.NPCState.Walk;
                break;
            case NPC.NPCState.Walk:
                nextStateType = NPC.NPCState.Idle;
                break;
            default:
                Debug.Log("Either transitions are undefined for current state, "+
                    "or wrong state added to Inspector");
                nextStateType = NPC.NPCState.Idle;
                break;
        }

        return nextStateType;
    }
}
