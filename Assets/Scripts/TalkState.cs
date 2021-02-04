using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State
{
    public float avgDuration = 4f;
    public float randDurMulti = 0.2f;

    public float timeStart = 0;
    [HideInInspector] public float duration; //doubles as current state duration
    public float counter = 0;//
    public float maxTalkDist = 0.5f;

    [HideInInspector] public bool isInitiator;
    public NPC closestNPC;
    [HideInInspector] public GameObject closestNPCGO;
    private TalkState closestTS;
    private NPC curNPC;
    private float closestInfSkill;
    private Response closestResponse;

    public override IEnumerator StartState(bool isDebug = false)
    {
        curNPC = NPC.NameToNPC(gameObject.name);

        if (isInitiator) {
            closestNPCGO = GetClosestNPCGO(); //already checked before FSM state entry
            closestTS = closestNPCGO.GetComponent<TalkState>();
            closestNPC = NPC.NameToNPC(closestNPCGO.name);

            if (isDebug) {
                DebugPrintState("I'm talking to "+closestNPCGO.name+", a "+closestNPC.type);
                DebugPrintState("Me vs audience I / Imax / IS / S / R: " + curNPC.influence + " / " + curNPC.influenceMax + " / "
                    + curNPC.influenceSkill + " / " + curNPC.state + " / " + curNPC.response + ", " + closestNPC.influence
                    + " / " + closestNPC.influenceMax + " / " + closestNPC.influenceSkill + " / " + closestNPC.state + " / " + closestNPC.response);
            }

            timeStart = Time.time;
            duration = avgDuration * Random.Range(1 - randDurMulti, 1 + randDurMulti);

            //sync TalkStates except isInitiator and closestNPCGO
            closestTS.timeStart = timeStart;
            closestTS.duration = duration;
            closestTS.counter = counter;
            closestTS.isInitiator = false;
            closestTS.closestNPCGO = gameObject;

            //get audience response
            closestInfSkill = closestNPC.influenceSkill;
            closestResponse = closestNPC.response;

            closestNPCGO.GetComponent<FSM>().ChangeState(NPC.NPCState.Talk,true);
        }

        if (isDebug) DebugPrintState("Talk started");
        yield break;
    }

    public override IEnumerator UpdateState(bool isDebug = false)
    {
        if (!isInitiator) {
            while (closestNPCGO == null && !Input.GetKeyDown("t")) yield return null; //audience wait for speaker to assign
            closestTS = closestNPCGO.GetComponent<TalkState>();
            closestNPC = NPC.NameToNPC(closestNPCGO.name);
            
            //get initiator response
            closestInfSkill = closestNPC.influenceSkill;
            closestResponse = closestNPC.response;
        }

        if (isDebug) DebugPrintState("Talking to "+closestNPCGO.name);
        counter = Time.time - timeStart;//
        if (Time.time - timeStart > duration ||
            (closestNPCGO != null && closestTS.closestNPCGO != gameObject)) { //audience got distracted
            isExit = true;
        } else yield return null;
    }

    public override void ExitState(bool isDebug = false, bool isImmediateExit = false)
    {
        if (isDebug) DebugPrintState("Exiting talk, isInitiator = "+isInitiator.ToString());

        //if initiator, get influenced less
        if (isInitiator) {
            curNPC.influence += (int)(duration * closestInfSkill * 0.5); //TODO % influence if talk cut short
            isInitiator = false;
        } else curNPC.influence += (int)(duration * closestInfSkill * 1);

        //convert if influenced
        if (curNPC.influence >= curNPC.influenceMax) { //TODO play poof
            // Response old = curNPC.response;
            curNPC.response = closestResponse;
            curNPC.influence = 0;
            // DebugPrintState("Changed their mind!");
        }

        if (isDebug) {
            DebugPrintState("As "+curNPC.type.ToString()+" after talk I/Imax/IS/R: " + curNPC.influence + " / " + curNPC.influenceMax + " / "
                + curNPC.influenceSkill + "/" + curNPC.response);
        }

        closestNPCGO = null;
        closestNPC = null;
        base.ExitState(isDebug, isImmediateExit);
    }

    //edited from: https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    public GameObject GetClosestNPCGO() //TODO move to state
    {
        Collider2D[] cldrs = Physics2D.OverlapCircleAll(transform.position, maxTalkDist);

        List<Transform> npcs = new List<Transform>();
        foreach (Collider2D c in cldrs) {
            if (c.transform != transform && c.transform.tag == "NPC") npcs.Add(c.transform);
        }
        if (npcs.Count == 0) return null;

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in npcs) {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget.gameObject;
    }
}
