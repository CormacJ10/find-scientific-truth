using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManger : MonoBehaviour
{
    public GameObject NPCContainer;
    public GameObject baseNpc;
    public GameObject smartNpc;
    public GameObject badNpc;
    public List<NPC> npcArray;

    // Smart NPCs are tracked because they're the end goal.
    public int smartNpcCount;
    public int smartNpcTotal;

    //UI

    //Response Text
    public GameObject responsetext1;
    public GameObject responsetext2;
    public GameObject responsetext3;
    public GameObject guessButton;

    [HideInInspector] public List<GameObject> responseList;


    // Start is called before the first frame update
    void Awake()
    {
        responseList = new List<GameObject>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
