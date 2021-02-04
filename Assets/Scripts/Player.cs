using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public variables.
    public float moveForce = 20f, jumpForce = 500f, maxVelocity = 4f;
    public Transform choicePanel;
    private LevelManger lmComp;

    private Rigidbody2D body;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        lmComp = GameObject.FindObjectOfType<LevelManger>();
    }

    private void FixedUpdate() {
        PlayerWalkKeyboard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e")) {
            Debug.Log("e pressed");
            GameObject go = gameObject.GetComponent<TalkState>().GetClosestNPCGO();
            // Debug.Log(go.name);
            if (go != null) {
                lmComp.ActivateChoice();
                lmComp.PopUpChoice(go);
            }
        }
    }

    void PlayerWalkKeyboard()
    {//Control Scheme for players if they use keyboard.
        float forceX = moveForce*Input.GetAxis("Horizontal");
        float forceY = moveForce*Input.GetAxis("Vertical");

        float h = Input.GetAxis("Horizontal");//Gives a number between 1(Right Arrow Key or 'D') and -1(Left Arrow Key or 'A').

        if (h > 0) {
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        } else if (h < 0) gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        
        body.AddForce(new Vector2(forceX, forceY));

        Vector2 curVel = body.velocity;
        curVel.x = Mathf.Clamp(curVel.x, -1 * maxVelocity, maxVelocity);
        curVel.y = Mathf.Clamp(curVel.y, -1 * maxVelocity, maxVelocity);
        body.velocity = curVel;
    }
}
