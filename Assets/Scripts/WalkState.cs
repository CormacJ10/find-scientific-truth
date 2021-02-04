using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{
    public float avgVel = 1;
    public float randVelMulti = 0.2f;
    public float avgDuration = 3;
    public float randDurMulti = 0.2f;

    public float timeStart = 0;
    private float duration;
    public float counter;//

    public override IEnumerator StartState(bool isDebug = false)
    {
        if (GetComponentInChildren<Rigidbody2D>() == null) {
            DebugPrintState(this.gameObject.name + ": Rigidbody2D not found");
            yield return null;
        }

        //random direction and velocity magnitude
        float random = Random.Range(0f, 2*Mathf.PI);
        Vector2 randDir = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        float velMag = avgVel * Random.Range(1-randVelMulti, 1+randVelMulti);

        GetComponentInChildren<Rigidbody2D>().mass = 1;
        GetComponentInChildren<Rigidbody2D>().velocity = randDir * velMag;
        if (isDebug) DebugPrintState("Walk started");
        timeStart = Time.time;
        duration = avgDuration * Random.Range(1-randDurMulti,1+randDurMulti);
        yield break;
    }

    public override IEnumerator UpdateState(bool isDebug = false)
    {
        // if (isDebug) DebugPrintState("Walking");
        counter = Time.time-timeStart;//
        if (Time.time - timeStart > duration) {
            if (isDebug) DebugPrintState("Walk ended naturally");//
            isExit = true;
        } else yield return null;
    }

    public override void ExitState(bool isDebug = false, bool isImmediateExit = false)
    {
        //stop walking
        GetComponentInChildren<Rigidbody2D>().velocity = Vector2.zero;
        GetComponentInChildren<Rigidbody2D>().mass = 1000000;
        base.ExitState(isDebug, isImmediateExit);
    }
}
