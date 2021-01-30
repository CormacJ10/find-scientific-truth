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
                    spawnedSmart++;
                } else if (spawnedBad < badCount) {
                    npc = badNpc;
                    spawnedBad++;
                }

                GameObject obj = GameObject.Instantiate(npc, rndPoint2D, Quaternion.identity);
                obj.transform.parent = NPCContainer.transform;
                
                NPCStats stats = obj.GetComponent<NPCStats>();
                NPC newNpc = new NPC(obj, npcType);
                npcArray.Add(newNpc);
                i++;
            }
        }

        baseNpc.SetActive(false);
        smartNpc.SetActive(false);
        badNpc.SetActive(false);
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
