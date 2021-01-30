using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isExit = false;

    //never start coroutines elsewhere
    //https://forum.unity.com/threads/ensure-that-theres-only-one-instance-of-a-coroutine-per-gameobject.545947/
    public void RunState(bool isDebug = false)
    {
        StopAllCoroutines();
        StartCoroutine(RunStateCoroutine(isDebug));
    }

    //Where the AI chooses what to do, splitting between wait and active periods
    private IEnumerator RunStateCoroutine(bool isDebug = false)
    {
        isExit = false;
        yield return StartCoroutine(StartState(isDebug));

        while (!isExit) {
            // if (isDebug) Debug.Log("State updating");
            yield return StartCoroutine(UpdateState(isDebug));
        }

        if (isDebug) Debug.Log("State ended"); //skipped if Run in same frame, or external Exit call
    }

    public abstract IEnumerator StartState(bool isDebug = false); //at least StartState and UpdateState must be implemented for a state

    public abstract IEnumerator UpdateState(bool isDebug = false);

    public virtual void ExitState(bool isImmediateExit = false) //if not overriden, simply exit
    {
        if (isImmediateExit) StopAllCoroutines(); //will skip "State ended" debug msg
        isExit = true;
    }
}
