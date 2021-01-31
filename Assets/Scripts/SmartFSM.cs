﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartFSM : FSM
{
    public int expIdleCountMax = 5;
    public int expIdleCount = 0;
    public float experimentPc = 0.2f;
    public bool isExperimenting;
    public float[] idlePcArray = { 0.2f, 0.5f, 0.3f, 0.0f };
    public float[] walkPcArray = { 0.0f, 0.3f, 0.7f, 0.0f };
    public float[] talkPcArray = { 0.7f, 0.3f, 0.0f, 0.0f };
    public float[] influencePcArray = { 0.5f, 0.0f, 0.5f, 0.0f };

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
                Debug.Log("Either transitions are undefined for current state, " +
                    "or no/wrong state added to Inspector");
                nextStateType = NPC.NPCState.Idle;
                break;
        }

        if (Random.value < experimentPc && !isExperimenting) {
            isExperimenting = true;
            nextStateType = NPC.NPCState.Idle;
            expIdleCount = expIdleCountMax - 1;
        } else if (isExperimenting && expIdleCount > 0) {
            nextStateType = NPC.NPCState.Idle;
            expIdleCount--;
        } else if (isExperimenting && expIdleCount == 0) {
            nextStateType = NPC.NPCState.Influence; //lecture ppl
            isExperimenting = false;
        }

        idlePcArray[3] += 0.005f; //thinks out argument better over time

        //do stuff if next state has conditions or depends on previous state
        switch (nextStateType)
        {
            case NPC.NPCState.Talk:
                TalkState tComp = GetComponent<TalkState>();

                if (tComp.GetClosestNPCGO() == null || NPC.NameToNPC(tComp.GetClosestNPCGO().name) == null)
                {
                    // DebugPrintFSM("No talking partner found");
                    nextStateType = NPC.NPCState.Idle;
                    break;
                }
                State closestState = tComp.GetClosestNPCGO().GetComponent<FSM>().curState;
                if (NPC.IsStateBlocking(NPC.StateToNPCEnum(closestState)))
                {
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
        if (altSprite != null && type == NPC.NPCType.Smart) {
            gameObject.GetComponent<SpriteRenderer>().sprite = altSprite;
            GameObject.FindObjectOfType<LevelManger>().smartNpcCount++;
        } //else do something
    }
}