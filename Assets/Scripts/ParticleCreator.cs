using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleCreator: MonoBehaviour
{
    [SerializeField]
    int StartingParticleCount=3;
    [SerializeField]
    int SpawningTime = 1;
    [SerializeField]
    Particle ParticlePrefab;
    [SerializeField]
    public Area SpawnArea;
    [SerializeField]
    Transform Parent;
    [SerializeField]
    public Particle[] StartedParticles;
    private void Start()
    {
        //Spawn(StartingParticleCount);
        foreach(Particle particle in  StartedParticles)
        {
            Spawn(particle);
        }
    }
    public Particle Spawn()
    {
        Particle particle = ParticlePrefab.Clone();
        return Spawn(particle);
    }
    public Particle Spawn(Particle particle)
    {
        float x, y;
        GetRandomPos(SpawnArea, out x, out y);
        return Spawn(particle,new Vector3(x,y,0));
    }
    public Particle Spawn(Particle particle,Vector3 position)
    {
        particle.BounceAction = new ExplodeBounce();
        Instantiate(particle, position, Quaternion.identity, Parent);
        return particle;
    }
    public void Spawn(int ParticleCount)
    {
        for (int i = 0; i < ParticleCount; i++)
        {
            Spawn();
        }
    }
    private void GetRandomPos(Area area,out float x,out float y)
    {
        x = UnityEngine.Random.Range(area.LeftUpCorner.localPosition.x, area.RightDownCorner.localPosition.x);
        y= UnityEngine.Random.Range(area.RightDownCorner.localPosition.y, area.LeftUpCorner.localPosition.y);
    }
}
[Serializable]
public class Area
{
    public Transform LeftUpCorner;
    public Transform RightDownCorner;
    //public float lx = 0; public float ly = 0;
    //public float rx = 0; public float ry=0;
}
