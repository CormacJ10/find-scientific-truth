using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using System.Math;s

public class LevelManger : MonoBehaviour
{
    public List<NPC> npcs;
    public GameObject baseNpc;
    public GameObject smartNpc;
    public GameObject badNpc;
    public List<NPC> npcList;
    // public List<String> quizList;

    // Smart NPCs are tracked because they're the end goal.
    public int smartNpcCount;
    public int smartNpcTotal;

    //UI
    public GameObject choicePanel;
    public GameObject gamePanel;
    public GameObject quizPanel;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject choiceGO;
    public Text totalScientistTxt;
    public Text totalScientistTxtShadow;
    public Transform cameraTransform;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.7f;
    private float dampingSpeed = 1.0f;
    Vector3 initialPosition;

    //Response Text
    public GameObject responsetext1;
    public GameObject responsetext2;
    public GameObject responsetext3;
    public GameObject guessButton;

    [HideInInspector] public List<GameObject> responseList;


    // Start is called before the first frame update
    void Awake()
    {
        // quizList = {{"Given a list of cities and the distances between each pair of cities, what is the shortest possible route that visits each city exactly once and returns to the origin city?", "What is the Travelling Salesman Problem?"}};

        responseList = new List<GameObject>();

        npcs = NPCSpawner.npcArray;

        smartNpcCount = 0;

        smartNpcTotal = GameObject.FindObjectOfType<NPCSpawner>().smartCount;

        totalScientistTxt.text = smartNpcCount.ToString() + " of "+ smartNpcTotal.ToString() + " Scientists Found!";
        totalScientistTxtShadow.text = smartNpcCount.ToString() + " of "+ smartNpcTotal.ToString() + " Scientists Found!";

        // Debug.Log(smartNpcTotal);
    }

    void OnEnable(){
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (choiceGO != null){
            choicePanel.transform.position = choiceGO.transform.position;
        }

        totalScientistTxt.text = smartNpcCount.ToString() + " of "+ smartNpcTotal.ToString() + " Scientists Found!";
        totalScientistTxtShadow.text = smartNpcCount.ToString() + " of "+ smartNpcTotal.ToString() + " Scientists Found!";
        if(smartNpcCount == smartNpcTotal){
            guessButton.SetActive(true);
        }

        if(shakeDuration > 0){
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else{
            shakeDuration = 0;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake(){
        shakeDuration = 2.0f;
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

    public void PopUpQuiz()
    {
        gamePanel.SetActive(false);
        quizPanel.SetActive(true);
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

    public void checkAnswer(Button button){
        string answer = button.GetComponent<Text>().text.Split(' ')[0];

        if(answer == "A."){
            winScreen.SetActive(true);
        }
        else{
            loseScreen.SetActive(true);
        }
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
