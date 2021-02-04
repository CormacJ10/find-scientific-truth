using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [HideInInspector] public float idleWaitMod = 1;
    [HideInInspector] public float idleCDMod = 1;
    private Coroutine idleCor;
    private Rigidbody2D rb;
    private Animator anim;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }
    void FixedUpdate()
    {
        //https://stackoverflow.com/questions/50446427/how-to-check-if-a-certain-animation-state-from-an-animator-is-running
        if (idleCor == null && rb.velocity.sqrMagnitude < 0.05f //should be around (min(avgVel*(1-velMulti)))^2 of all NPCs
            && anim.GetCurrentAnimatorStateInfo(0).IsName("Blank")) {
            idleCor = StartCoroutine(RandomIdle());
        }
    }

    private IEnumerator RandomIdle()
    {
        yield return new WaitForSeconds(Random.Range(3, 7) * idleWaitMod);

        //check if still stationary after rand time
        if (rb.velocity.sqrMagnitude < 0.05f
            && anim.GetCurrentAnimatorStateInfo(0).IsName("Blank")) {
            anim.SetTrigger("StartIdle");
        }

        yield return new WaitForSeconds(Random.Range(8, 15) * idleCDMod);
        
        anim.ResetTrigger("StartIdle");
        idleCor = null;
        yield break;
    }

    public void UpdateAnimator(float idleWaitMod, float idleCDMod)
    {
        if (idleCor != null) {
            StopCoroutine(idleCor);
            idleCor = null;
        }

        anim = gameObject.GetComponentInChildren<Animator>();
        this.idleWaitMod = idleWaitMod;
        this.idleCDMod = idleCDMod;
    }
}
