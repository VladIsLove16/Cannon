using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInfo
{
    public Hitter Hitter;
    public Player Emitter;
    public HitInfo(Hitter Hitter, Player emitter)
    {
        this.Hitter = Hitter;
        Emitter = emitter;
    }   
}
