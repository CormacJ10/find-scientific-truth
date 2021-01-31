using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class FSM : MonoBehaviour
{
    public List<State> stateList;
    public State curState;
    public bool isStateRunning = false;
    public bool isDebug = false;

    //
    public Sprite altSprite;
    //

    // Start is called before the first frame update
    void Start()
    {
        stateList = new List<State>();
        stateList = GetComponents<State>().ToList<State>(); //make sure no dupes

        curState = stateList[0]; //whichever state that's top in inspector is starting state
        StartCoroutine(RunFSM());
    }

    //FSM runs whatever active state's UpdateState()
    public IEnumerator RunFSM()
    {
        // Debug.Log(gameObject.name+": RunFSM started");
        while (true) {
            if (!isStateRunning) {
                curState.RunState(isDebug);
                isStateRunning = true;
            }

            yield return null;
            if (curState.isExit) Transition();
        }
    }

    public void ChangeState(NPC.NPCState nextState, bool isImmediateExit = false)
    {
        curState.ExitState(isDebug, isImmediateExit);
        if (isDebug) DebugPrintFSM("Transitioning to "+nextState.ToString()+"State");
        isStateRunning = false;

        for (int i = 0; i<stateList.Count; i++) {
            System.Type nextStateScript = System.Type.GetType(nextState.ToString()+"State");

            if (stateList[i].GetType() == nextStateScript) {
                curState = stateList[i];
                if (isImmediateExit) {
                    StopAllCoroutines();
                    StartCoroutine(RunFSM());
                }
                return;
            }
        }
        //if no matching state script found
        Debug.Log("nextState component ("+nextState.ToString()+") not added");
    }

    //Decide next state then change state
    public void Transition()
    {
        if (isDebug) DebugPrintFSM("State finished naturally");
        NPC.NPCState curStateType = NPC.StateToNPCEnum(curState);
        ChangeState(PickNextState(curStateType));
    }

    public abstract NPC.NPCState PickNextState(NPC.NPCState curStateType);


    //edited from: https://docs.unity3d.com/560/Documentation/Manual/RandomNumbers.html
    public NPC.NPCState RNGChoose(float[] probs)
    {
        float total = 0;
        foreach (float elem in probs) total += elem;

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return (NPC.NPCState)i;
            }
            else randomPoint -= probs[i];
        }
        return (NPC.NPCState)(probs.Length - 1);
    }

    public void DebugPrintFSM(string msg = null)
    {
        Debug.Log(gameObject.name + "[" + curState.GetType().ToString() + ", "
            + Time.frameCount + ", " + Time.time + "s] " + msg);
    }

    public abstract void Reveal(NPC.NPCType type);
}