using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class FSM : MonoBehaviour
{
    public List<State> stateList;
    public State curState;
    public bool isStateRunning = false;

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
        while (!Input.GetKeyDown("z")) {
            if (!isStateRunning) {
                curState.RunState();
                isStateRunning = true;
            }

            if (curState.isExit) Transition();

            yield return null;
        }
        // Debug.Log(gameObject.name + ": FSM ended");
        yield return null;
    }

    public void ChangeState(NPC.NPCState nextState)
    {
        curState.ExitState();
        isStateRunning = false;

        for (int i = 0; i<stateList.Count; i++) {
            System.Type nextStateScript = System.Type.GetType(nextState.ToString()+"State");

            if (stateList[i].GetType() == nextStateScript) {
                curState = stateList[i];
                curState.RunState();
                return;
            }
        }
        //if no matching state script found
        Debug.Log("nextState component ("+nextState.ToString()+") not added");
    }

    //Decide next state then change state
    public void Transition()
    {
        // string curStateType = curState.GetType().ToString();
        
        // switch (curStateType)
        // {
        //     case "IdleState":
        //         nextState = NPC.NPCState.Walk;
        //         break;
        //     case "WalkState":
        //         nextState = NPC.NPCState.Idle;
        //         break;
        //     default:
        //         Debug.Log("Either transitions are undefined for current state, "+
        //             "or wrong state added to Inspector");
        //         nextState = NPC.NPCState.Idle;
        //         break;
        // }
        
        NPC.NPCState curStateType = NPC.StateToNPCEnum(curState);

        ChangeState(PickNextState(curStateType));
    }

    public abstract NPC.NPCState PickNextState(NPC.NPCState curStateType);
}