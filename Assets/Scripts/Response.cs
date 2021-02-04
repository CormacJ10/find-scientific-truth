using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Response
{
    //State variable
    public enum ResponseType
    {
        Boolean, //tick/cross (spriteName)
        Number, //number, colour
        Word, //word/letter, colour
        Symbol, //symbol (spriteName), rotation, colour
        Shape, //shape (spriteName), rotation, colour
        Person //think street signpost simple: job/pose (spriteName), rotation, colour
    }

    private string spriteName = "unnamed";
    
    public Sprite sprite;
    public bool boolean;
    public float num;
    public string word;
    public float rot;
    public Color col;
    public Color bg; //if colour is bright => dark background, and vice versa

    // public Response(ResponseType type, string spriteName, bool boolean, float num, string word, float rot, Color col)
    public Response(string guess)
    {
        // Sprite spr = Resources.Load<Sprite>("Sprites/" + spriteName);
        // if (spr == null) {
        //     Debug.Log(spriteName + " does not exist in Resources/Sprites folder");
        // } else {
        //     this.sprite = spr;
        // }
        this.spriteName = null;
        this.boolean = false;
        this.num = 0;
        this.word = guess;
        this.rot = 0;
        this.col = Color.black;

        if (col.grayscale > 0.5f) {
            bg = Color.black;
        } else {
            bg = Color.white;
        }
    }

    public override string ToString()
    {
        return spriteName+" (Response)";
    }
}
