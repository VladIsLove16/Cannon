using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactExplosion : Explosion
{
    [SerializeField]
    public int Delay = 10000;
    protected override void Update()
    {
        base.Update();
        if(Delay>0)
            Delay-=Mathf.RoundToInt(Time.deltaTime*1000);
    }
    public ContactExplosion()
    {
        ExplodeNearExplosions = true;
    }
    protected void OnCollisionEnter(Collision collision)
    {
        if(Delay>0)return;
        Explode(Emmiter,this);
    }
}
