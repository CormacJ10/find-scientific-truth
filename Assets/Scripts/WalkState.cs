﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{
    public PolygonCollider2D playBounds;
    public float avgVel = 1;
    public float randVelMulti = 0.2f;
    public float avgDuration = 3;
    public float randDurMulti = 0.2f;

    public float timeStart = 0;
    private float duration;
    public float counter;//

    public override IEnumerator StartState(bool isDebug = false)
    {
        if (GetComponent<Rigidbody2D>() == null) {
            Debug.Log(this.gameObject.name + ": Rigidbody2D not found");
            yield return null;
        }

        //random direction and velocity magnitude
        float random = Random.Range(0f, 2*Mathf.PI);
        Vector2 randDir = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        float velMag = avgVel * Random.Range(1-randVelMulti, 1+randVelMulti);

        GetComponent<Rigidbody2D>().velocity = randDir * velMag;
        if (isDebug) Debug.Log("Walk started");
        timeStart = Time.time;
        duration = avgDuration * Random.Range(1-randDurMulti,1+randDurMulti);
        yield return null;
    }

    public override IEnumerator UpdateState(bool isDebug = false)
    {
        if (isDebug) Debug.Log("Walking");
        counter = Time.time-timeStart;//
        if (Time.time - timeStart > duration) {
            // Debug.Log("Walk ended naturally");//
            ExitState();
        } else yield return null;
    }

    public override void ExitState(bool isImmediateExit = false)
    {
        //stop walking
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        base.ExitState(isImmediateExit);
    }

    // private void OnTriggerEnter2D(Collider2D other) {
    //     if (other.transform.tag == "PlayBounds") {
    //         Debug.Log(gameObject.name+" has hit "+other.gameObject.name);
    //         GetComponent<Rigidbody2D>().velocity = -1 * GetComponent<Rigidbody2D>().velocity;
    //     }
    // }
}