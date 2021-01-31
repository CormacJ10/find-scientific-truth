using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManger : MonoBehaviour
{
    public List<NPC> npcs;
    public GameObject baseNpc;
    public GameObject smartNpc;
    public GameObject badNpc;
    public List<NPC> npcList;

    // Smart NPCs are tracked because they're the end goal.
    public int smartNpcCount;
    public int smartNpcTotal;

    //UI
    public GameObject choicePanel;
    public GameObject gamePanel;
    public GameObject choiceGO;
    public Text totalScientistTxt;

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

        npcs = NPCSpawner.npcArray;

        smartNpcCount = 0;

        smartNpcTotal = GameObject.FindObjectOfType<NPCSpawner>().smartCount;

        totalScientistTxt.text = smartNpcCount.ToString() + " of "+ smartNpcTotal.ToString() + " Scientists Found!";

        // Debug.Log(smartNpcTotal);
    }

    // Update is called once per frame
    void Update()
    {
        if (choiceGO != null){
            choicePanel.transform.position = choiceGO.transform.position;
        }

        while(smartNpcCount != smartNpcTotal){
            totalScientistTxt.text = smartNpcCount.ToString() + " of "+ smartNpcTotal.ToString() + "Scientists Found";
            if(smartNpcCount == smartNpcTotal){
                guessButton.gameObject.SetActive(true);
            }
        }
    }

    public void ActivateChoice(){
        choicePanel.SetActive(true);
    }

    public void PopUpChoice(GameObject choiceGO)
    {
        this.choiceGO = choiceGO;
        // Debug.Log("choice world location: "+choiceGO.transform.position.ToString()+", ScrnPt: "
        //     +Camera.main.WorldToScreenPoint(choiceGO.transform.position).ToString()+ ", ViewPPt: "
        //     + Camera.main.WorldToViewportPoint(choiceGO.transform.position).ToString()+ ", UIPt: "
        //     + worldToUISpace(GameObject.FindObjectOfType<Canvas>(), choiceGO.transform.position).ToString());

        // Debug.Log("panel world location: " + choicePanel.GetComponent<RectTransform>().anchoredPosition.ToString() + ", ScrnPt: "
        //     + Camera.main.WorldToScreenPoint(choicePanel.GetComponent<RectTransform>().anchoredPosition).ToString() + ", ViewPPt: "
        //     + Camera.main.WorldToViewportPoint(choicePanel.GetComponent<RectTransform>().anchoredPosition).ToString() + ", UIPt: "
        //     + worldToUISpace(GameObject.FindObjectOfType<Canvas>(), choicePanel.GetComponent<RectTransform>().anchoredPosition).ToString());
    }

    public void ChoiceButtonPressSmart()
    {
        ChoiceButtonPress(NPC.NPCType.Smart);
    }

    public void ChoiceButtonPressBad()
    {
        ChoiceButtonPress(NPC.NPCType.Bad);
    }

    public void ChoiceButtonPressSpot()
    {
        // choicePanel.transform.parent.gameObject.GetComponent<NPCStats>().SwitchSpotlight();
        choiceGO.GetComponent<NPCStats>().SwitchSpotlight();
    }

    public void ChoiceButtonPress(NPC.NPCType type)
    {
        // choicePanel.transform.parent.gameObject.GetComponent<FSM>().Reveal(type);
        choiceGO.GetComponent<FSM>().Reveal(type);
    }

    //https://stackoverflow.com/questions/45046256/move-ui-recttransform-to-world-position
    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
