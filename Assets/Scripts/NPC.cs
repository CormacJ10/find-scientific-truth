using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    //State variable
    public enum NPCState
    {
        Idle,
        Walk,
        Talk,
        Influence
    }

    public enum NPCType
    {
        Base,
        Smart,
        Bad
    }

    //Unique IDs: https://stackoverflow.com/questions/8813435/incrementing-a-unique-id-number-in-the-constructor
    private static int counter = 0;
    private int ID { get; set; }
    public GameObject GO { get; }
    public float maxVel { get; } = 5;
    public int influence { get; }
    public int influenceSkill { get; set;} = 5;
    public NPCState state { get; set;} = NPCState.Idle; //set state after spawn, using npc-type FSM
    public NPCType type { get; set;}
    public Response response { get; set;}

    public NPC(GameObject GO, NPCType type)
    {
        this.ID = System.Threading.Interlocked.Increment(ref counter);
        this.GO = GO;
        this.maxVel = maxVel;
        this.influence = influence;
        this.influenceSkill = influenceSkill;
        this.state = state;
        this.type = type;
        this.response = response;
    }

    public override string ToString()
    {
        return "NPC "+ID.ToString();
    }

    //for opposite conversion refer to ChangeState in FSM.cs
    public static NPCState StateToNPCEnum(State s)
    {
        string stateName = s.GetType().ToString();
        stateName = stateName.Remove(stateName.Length - 5); //remove "State" suffix

        object enumObj = System.Enum.Parse(typeof(NPCState), stateName);

        return (NPCState)enumObj;
    }
}
