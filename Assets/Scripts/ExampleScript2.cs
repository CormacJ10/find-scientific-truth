using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        State state = this.gameObject.AddComponent<IdleState>();
        Debug.Log(state.GetType().ToString());
        Debug.Log(state.GetType().ToString().Remove(state.GetType().ToString().Length-5));
        Debug.Log(((NPC.NPCState)System.Enum.Parse(typeof(NPC.NPCState),"Idle")).ToString());

        // Debug.Log(state.GetType() == System.Type.GetType("IdleState"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
