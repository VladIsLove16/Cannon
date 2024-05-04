using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatingBounce : IBounce
{
    public void Execute(Transform BounceTransform)
    {
        ParticleCreator particleCreator = new ParticleCreator();
        Particle parcticle= particleCreator.Spawn();
        parcticle.transform.position = BounceTransform.position;
    }
}
