using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript2 : MonoBehaviour
{
    public float[] idlePcArray = { 0.0f, 0.5f, 0.4f, 0.1f };

    // Start is called before the first frame update
    void Start()
    {
        // State state = this.gameObject.AddComponent<IdleState>();
        // Debug.Log(state.GetType().ToString());
        // Debug.Log(state.GetType().ToString().Remove(state.GetType().ToString().Length-5));
        // Debug.Log(((NPC.NPCState)System.Enum.Parse(typeof(NPC.NPCState),"Idle")).ToString());

        // Debug.Log(state.GetType() == System.Type.GetType("IdleState"));
        // int zero = 0;
        // int one = 0;
        // int two = 0;
        // int three = 0;
        // for(int i = 0;i<10000;i++) {
        //     int val = Choose(idlePcArray);

        //     if (val == 0) zero++;
        //     if (val == 1) one++;
        //     if (val == 2) two++;
        //     if (val == 3) three++;
        // }

        // Debug.Log("zero, one, two, three count: "+zero.ToString()+", "+
        //     one.ToString()+ ", " + two.ToString()+ ", " + three.ToString());

        string n = "NPC 99";
        Debug.Log(n);
        Debug.Log(n.Contains("Test"));
        Debug.Log(n.Contains("NPC "));
        Debug.Log(n.Substring(3));
        Debug.Log(n.Substring(n.IndexOf(" ")));
        Debug.Log(n.Substring(n.IndexOf(" ") + 1));
        Debug.Log(n.Substring(n.IndexOf(" ") + 2));

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetClosestNPC().name);
    }

    public int Choose(float[] probs)
    {
        float total = 0;
        foreach (float elem in probs) total += elem;

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else randomPoint -= probs[i];
        }
        return probs.Length - 1;
    }

    //edited from: https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    Transform GetClosestNPC()
    {
        Collider2D[] cldrs = Physics2D.OverlapCircleAll(transform.position, 5); //maxDist

        List<Transform> npcs = new List<Transform>();
        foreach (Collider2D c in cldrs) {
            if (c.transform != transform && c.transform.tag == "NPC") npcs.Add(c.transform);
        }

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in npcs)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
