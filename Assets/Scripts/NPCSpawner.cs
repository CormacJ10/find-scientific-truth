using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class to spawn 2D objects inside an Polygon Collider 2D area for forum.unity.com by Zer0Cool
/// - create an empty GameObject
/// - add a "Polygon Collider 2D" component
/// - edit the vertices of the polygon collider to match your spawning zone (see https://docs.unity3d.com/Manual/class-PolygonCollider2D.html)
/// - EDITED from https://forum.unity.com/threads/spawn-object-in-certain-zone-2d.930684/
/// </summary>
public class NPCSpawner : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public GameObject NPCContainer;
    public GameObject baseNpc;
    public GameObject smartNpc;
    public GameObject badNpc;
    public GameObject responseTemplate;


    [HideInInspector] public static List<NPC> npcArray;
    public int baseCount = 10;
    public int smartCount = 5;
    public int badCount = 5;

    void Awake()
    {
        if (polygonCollider == null) GetComponent<PolygonCollider2D>();
        if (polygonCollider == null) Debug.Log("Please assign PolygonCollider2D component.");

        int i = 0;
        int spawnedSmart = 0;
        int spawnedBad = 0;
        npcArray = new List<NPC>();
        while (i < (baseCount+smartCount+badCount)) {
            Vector3 rndPoint3D = RandomPointInBounds(polygonCollider.bounds, 1f);
            Vector2 rndPoint2D = new Vector2(rndPoint3D.x, rndPoint3D.y);
            Vector2 rndPointInside = polygonCollider.ClosestPoint(new Vector2(rndPoint2D.x, rndPoint2D.y));

            if (rndPointInside.x == rndPoint2D.x && rndPointInside.y == rndPoint2D.y) {
                GameObject npc = baseNpc;
                NPC.NPCType npcType = NPC.NPCType.Base;

                if (spawnedSmart < smartCount) {
                    npc = smartNpc;
                    npcType = NPC.NPCType.Smart;
                    spawnedSmart++;
                } else if (spawnedBad < badCount) {
                    npc = badNpc;
                    npcType = NPC.NPCType.Bad;
                    spawnedBad++;
                }

                GameObject obj = GameObject.Instantiate(npc, rndPoint2D, Quaternion.identity);
                obj.transform.parent = NPCContainer.transform;
                obj.name = "NPC "+i.ToString();

                List<string> guesses = new List<string>(); //TODO possible guesses
                string answer = "A"; //TODO must input answer as A
                Response resp = NPC.GenerateResponse(guesses, answer, npcType);
                
                NPCStats stats = obj.GetComponent<NPCStats>();
                NPC newNpc = new NPC(obj, npcType);
                newNpc.response = resp;
                npcArray.Add(newNpc);
                i++;
            }
        }

    StartCoroutine(DeactivateTemplates());
    }

    private IEnumerator DeactivateTemplates()
    {
        // yield return new WaitForSeconds(0.01f);

        baseNpc.SetActive(false);
        smartNpc.SetActive(false);
        badNpc.SetActive(false);
        // responseTemplate.SetActive(false);

        yield return null;
    }

    public List<NPC> getNPCList(){
        return npcArray;
    }

    private Vector3 RandomPointInBounds(Bounds bounds, float scale)
    {
        return new Vector3(
            Random.Range(bounds.min.x * scale, bounds.max.x * scale),
            Random.Range(bounds.min.y * scale, bounds.max.y * scale),
            Random.Range(bounds.min.z * scale, bounds.max.z * scale)
        );
    }
}
