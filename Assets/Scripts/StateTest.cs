using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTest : MonoBehaviour
{
    State state;
    State state2;
    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<State>();
        state2 = GetComponents<State>()[1];
        DebugPrint("Testing top state: "+state.GetType().ToString());

        StartCoroutine(TestState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TestState()
    {
        State curState = state;

        // PrintFrameTime("Before Start");
        // yield return StartCoroutine(state.StartState());
        // PrintFrameTime("After Start");

        DebugPrint("Before Run");
        curState.RunState(true);
        DebugPrint("After Run");

        // yield return new WaitForSeconds(1);
        // state.ExitState();
        // DebugPrint("After Exit");

        // yield return new WaitForSeconds(1);
        // state.RunState(true);
        // DebugPrint("After Run2");

        // yield return new WaitForSeconds(2);
        // curState.ExitState();
        // curState = state2;
        // DebugPrint("Before Run2");
        // curState.RunState(true);
        // DebugPrint("After Run2");

        // yield return new WaitForSeconds(2);
        // curState.ExitState();
        // curState = state;
        // DebugPrint("Before Run3");
        // curState.RunState(true);
        // DebugPrint("After Run3");

        yield return null;
    }

    public void DebugPrint(string msg = null)
    {
        Debug.Log(gameObject.name+"["+state.GetType().ToString()+", "
            +Time.frameCount+", "+Time.time+"s] "+msg);
    }
}
