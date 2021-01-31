using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM : FSM
{
    public float[] idlePcArray = {0.0f,0.5f,0.4f,0.1f};
    public float[] walkPcArray = {0.5f,0.2f,0.3f,0.0f};
    public float[] talkPcArray = {0.7f,0.3f,0.0f,0.0f};
    public float[] influencePcArray = {0.9f,0.0f,0.1f,0.0f};
    // public float[] idlePcArray = {0,1.0f,0.0f,0.0f};
    // public float[] walkPcArray = {1,0,0,0};
    // public float[] talkPcArray = {1,0,0,0};
    // public float[] influencePcArray = {1,0,0,0};


    public override NPC.NPCState PickNextState(NPC.NPCState curStateType)
    {
        NPC.NPCState nextStateType;
        switch (curStateType)
        {
            case NPC.NPCState.Idle:
                nextStateType = RNGChoose(idlePcArray);
                break;
            case NPC.NPCState.Walk:
                nextStateType = RNGChoose(walkPcArray);
                break;
            case NPC.NPCState.Talk:
                nextStateType = RNGChoose(talkPcArray);
                break;
            case NPC.NPCState.Influence:
                nextStateType = RNGChoose(influencePcArray);
                break;
            default:
                Debug.Log("Either transitions are undefined for current state, "+
                    "or no/wrong state added to Inspector");
                nextStateType = NPC.NPCState.Idle;
                break;
        }

        //do stuff if next state has conditions or depends on previous state
        switch (nextStateType)
        {
            case NPC.NPCState.Talk:
                TalkState tComp = GetComponent<TalkState>();
                
                if (tComp.GetClosestNPCGO() == null || NPC.NameToNPC(tComp.GetClosestNPCGO().name) == null) {
                    // DebugPrintFSM("No talking partner found");
                    nextStateType = NPC.NPCState.Idle;
                    break;
                }
                State closestState = tComp.GetClosestNPCGO().GetComponent<FSM>().curState;
                if (NPC.IsStateBlocking(NPC.StateToNPCEnum(closestState))) {
                    // DebugPrintFSM(tComp.GetClosestNPCGO().name+" in blocking state "+closestState.GetType().ToString());
                    nextStateType = NPC.NPCState.Idle;
                    break;
                }

                tComp.isInitiator = true;
                break;
        }

        return nextStateType;
    }

    public override void Reveal(NPC.NPCType type)
    {
        //do something bad
    }
}
