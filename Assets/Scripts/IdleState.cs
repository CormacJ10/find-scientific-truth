using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public float avgDuration = 1.5f;
    public float randDurMulti = 0.2f;

    private float timeStart = 0;
    private float duration;
    public float counter; //

    public override IEnumerator StartState(bool isDebug = false)
    {
        timeStart = Time.time;
        duration = avgDuration * Random.Range(1-randDurMulti,1+randDurMulti);
        if (isDebug) DebugPrintState("Idle started");
        yield break;
    }

    public override IEnumerator UpdateState(bool isDebug = false)
    {
        // if (isDebug) DebugPrintState("Idling");
        counter = Time.time - timeStart;//
        if (Time.time - timeStart > duration) {
            isExit = true;
        } else yield return null;
    }
}
