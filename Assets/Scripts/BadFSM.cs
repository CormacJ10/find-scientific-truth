using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadFSM : FSM
{
    public int prepTally = 0;
    public int prepTallyMax = 5;
    public float experimentPc = 0.2f;
    public float[] idlePcArray = { 0.2f, 0.1f, 0.3f, 0.4f };
    public float[] walkPcArray = { 0.7f, 0.3f, 0.0f, 0.0f };
    public float[] talkPcArray = { 0.2f, 0.0f, 0.4f, 0.4f };
    public float[] influencePcArray = { 1.0f, 0.0f, 0.0f, 0.0f };

    public GameObject choicePanel;

    public GameObject choice_bubble_1;
    public GameObject choice_bubble_2;
    public GameObject choice_bubble_3;
    public GameObject baseGO;
    public GameObject trueGO;
    public Animator poofAnim;

    public override void Start()
    {
        trueGO.SetActive(false);
        baseGO.SetActive(true);
        base.Start();
    }

    public override NPC.NPCState PickNextState(NPC.NPCState curStateType)
    {
        // NPC.NPCState nextStateType;
        // switch (curStateType)
        // {
        //     case NPC.NPCState.Idle:
        //         nextStateType = RNGChoose(idlePcArray);
        //         break;
        //     case NPC.NPCState.Walk:
        //         nextStateType = RNGChoose(walkPcArray);
        //         break;
        //     case NPC.NPCState.Talk:
        //         nextStateType = RNGChoose(talkPcArray);
        //         break;
        //     case NPC.NPCState.Influence:
        //         nextStateType = RNGChoose(influencePcArray);
        //         break;
        //     default:
        //         Debug.Log("Either transitions are undefined for current state, " +
        //             "or no/wrong state added to Inspector");
        //         nextStateType = NPC.NPCState.Idle;
        //         break;
        // }

        // if (prepTally == 5) {
        //     nextStateType = NPC.NPCState.Influence;
        //     NPC.NameToNPC(gameObject.name).influenceSkill += 5;
        //     prepTally = 0;
        // } else if (nextStateType == NPC.NPCState.Idle) {
        //     prepTally += 2;
        // } else if (nextStateType == NPC.NPCState.Talk) {
        //     prepTally += 1;
        // } else if (nextStateType == NPC.NPCState.Influence) {
        //     prepTally -= 1;
        // }

        // prepTally = Mathf.Clamp(prepTally, 0, prepTallyMax);
        // idlePcArray[3] = prepTally*0.4f;
        // walkPcArray[3] = prepTally*0.3f;
        // talkPcArray[3] = prepTally*0.2f;
        // influencePcArray[3] = prepTally*0.1f;

        // //do stuff if next state has conditions or depends on previous state
        // switch (nextStateType)
        // {
        //     case NPC.NPCState.Talk:
        //         TalkState tComp = GetComponent<TalkState>();

        //         if (tComp.GetClosestNPCGO() == null || NPC.NameToNPC(tComp.GetClosestNPCGO().name) == null)
        //         {
        //             // DebugPrintFSM("No talking partner found");
        //             nextStateType = NPC.NPCState.Idle;
        //             break;
        //         }
        //         State closestState = tComp.GetClosestNPCGO().GetComponent<FSM>().curState;
        //         if (NPC.IsStateBlocking(NPC.StateToNPCEnum(closestState)))
        //         {
        //             // DebugPrintFSM(tComp.GetClosestNPCGO().name+" in blocking state "+closestState.GetType().ToString());
        //             nextStateType = NPC.NPCState.Idle;
        //             break;
        //         }

        //         tComp.isInitiator = true;
        //         break;
        // }

        // return nextStateType;

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
        if (type == NPC.NPCType.Bad) {
            baseGO.SetActive(false);
            trueGO.SetActive(true);
            poofAnim.SetTrigger("Reveal");
            gameObject.GetComponent<AnimController>().UpdateAnimator(0.5f, 0.2f);
        } //else do something
    }
}
