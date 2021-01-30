﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    public int influence = 0; //starting influence
    public int influenceMax = 100;
    public int influenceSkill = 5;
    public NPC.NPCType type = NPC.NPCType.Base;
    public int spotlightEffect = 2;
    public bool isSpotlighted;

    private void Update() {
        NPC curNPC;
        if (NPC.NameToNPC(gameObject.name) != null) {
            curNPC = NPC.NameToNPC(gameObject.name);
            influence = curNPC.influence;
            
            if (isSpotlighted) curNPC.influenceSkill = influenceSkill*spotlightEffect;
        }
    }

    public void SwitchSpotlight()
    {
        isSpotlighted = !isSpotlighted; //TODO lights anim
    }
}
