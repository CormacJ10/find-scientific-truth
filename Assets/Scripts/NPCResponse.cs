using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCResponse : MonoBehaviour
{
    public GameObject template;
    public GameObject responseContainer;
    private GameObject response;
    private Rigidbody2D rb;
    private Rigidbody2D playerRB;
    private List<Transform> childs; 
    
    // Start is called before the first frame update
    void Start()
    {
        response = GameObject.Instantiate(template, Vector2.zero, Quaternion.identity, responseContainer.transform);
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerRB = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        
        childs = new List<Transform>();
        foreach (Transform child in response.transform) childs.Add(child);
    }

    // Update is called once per frame
    void Update()
    {
        response.transform.position = transform.position + new Vector3(-1,1,0);
        if (NPC.NameToNPC(gameObject.name) != null) {
            string resp = NPC.NameToNPC(gameObject.name).response.word;
            ChangeValue(resp);
            
            if ((playerRB.position - rb.position).sqrMagnitude < 8f) { //may hit performance too much
                response.GetComponent<SpriteRenderer>().enabled = true;
                for (int i=0;i<childs.Count;i++) {
                    if (resp == childs[i].name) childs[i].gameObject.SetActive(true);
                }
            } else {
                response.GetComponent<SpriteRenderer>().enabled = false;

                for (int i=0;i<childs.Count;i++) {
                    if (resp == childs[i].name) childs[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ChangeValue(string newString)
    {
        for (int i=0;i<childs.Count;i++) {
            if (newString == childs[i].name) {
                childs[i].gameObject.SetActive(true);
            } else childs[i].gameObject.SetActive(false);
        }
    }
}
