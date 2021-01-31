using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceState : State
{
    public float avgDuration = 6f;
    public float randDurMulti = 0.2f;

    public float timeStart = 0;
    [HideInInspector] public float duration; //doubles as current state duration
    public float counter = 0;//
    public float maxInflDist = 1.5f;
    public float assignInterval = 1;
    public float relistenPc = 0.2f;
    public int bias = 10; //each influence decreases influencer's influence :D
    private NPC curNPC;
    private List<GameObject> audienceGOs;
    private Coroutine repCor;

    public override IEnumerator StartState(bool isDebug = false)
    {
        curNPC = NPC.NameToNPC(gameObject.name);
        audienceGOs = new List<GameObject>();

        timeStart = Time.time;
        duration = avgDuration * Random.Range(1 - randDurMulti, 1 + randDurMulti);
        repCor = StartCoroutine(RepeatTrySet(AoeGetNearestNPCGOs(),duration));

        if (isDebug) DebugPrintState("Influence started");
        yield break;
    }

    public override IEnumerator UpdateState(bool isDebug = false)
    {
        // if (isDebug) DebugPrintState("Influencing");
        counter = Time.time - timeStart;//
        if (Time.time - timeStart > duration) {
            isExit = true;
        } else yield return null;
    }

    public override void ExitState(bool isDebug = false, bool isImmediateExit = false)
    {
        curNPC.influence -= bias;
        
        StopCoroutine(repCor);
        audienceGOs = null;
        if (isDebug) DebugPrintState("Influence ended");
        base.ExitState(isDebug, isImmediateExit);
    }

    public List<GameObject> AoeGetNearestNPCGOs()
    {
        float aoeSkillEffect = Mathf.Log10(curNPC.influenceSkill); //more skill = more influeced NPCs
        Collider2D[] cldrs = Physics2D.OverlapCircleAll(transform.position, maxInflDist + aoeSkillEffect);
        // DebugPrintState("cldrs len: "+cldrs.Length.ToString());
        if (cldrs.Length == 0) return null;

        List<GameObject> npcs = new List<GameObject>();
        foreach (Collider2D c in cldrs) {
            if (c.transform != transform && c.transform.tag == "NPC" && c.gameObject.activeInHierarchy) {
                State closestState = c.gameObject.GetComponent<FSM>().curState;
                if (!NPC.IsStateBlocking(NPC.StateToNPCEnum(closestState))) {
                    // DebugPrintState(c.gameObject.name+" added to npcs");
                    npcs.Add(c.gameObject);
                }
            }
        }
        if (npcs.Count == 0) {
            return null;
        } else return npcs;
    }

    public void SetAttention(GameObject go, float duration)
    {
        //hacky algo
        float attnDur = 100*assignInterval/duration+Mathf.Log10(curNPC.influenceSkill);
        
        TalkState ts = go.GetComponent<TalkState>();
        ts.timeStart = timeStart;
        ts.duration = attnDur * Random.Range(1 - randDurMulti, 1 + randDurMulti);
        ts.counter = counter;
        ts.isInitiator = false;
        ts.closestNPCGO = gameObject;

        go.GetComponent<FSM>().ChangeState(NPC.NPCState.Talk, true);
    }

    public void TrySetAttentions(List<GameObject> gos, float duration)
    {
        if (gos == null) return;
        // DebugPrintState("TrySetting "+gos.Count.ToString()+" gameobjects");

        foreach (GameObject g in gos) {
            if (!audienceGOs.Contains(g)) {
                SetAttention(g, duration);
                audienceGOs.Add(g);
            } else if (Random.value < relistenPc) SetAttention(g, duration);
        }
    }

    private IEnumerator RepeatTrySet(List<GameObject> gos, float duration)
    {
        while (!Input.GetKey("u")) {
            TrySetAttentions(gos,duration);
            yield return new WaitForSeconds(assignInterval);
        }
    }
}
