using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleCreator: MonoBehaviour
{
    [SerializeField]
    int SpawningTime = 100;
    [SerializeField]
    Particle RunTimeParticlePrefab;
    [SerializeField]
    public Area SpawnArea;
    [SerializeField]
    Transform Parent;
    [SerializeField]
    public Particle[] StartedParticles;
    public bool StartSpawning=false;
    private void Start()
    {
        foreach(Particle particle in  StartedParticles)
        {
            Spawn(particle);
        }
        StartCoroutine(RunTimeSpawn());
    }
    private IEnumerator RunTimeSpawn()
    {
        while (true)
        {
            if (StartSpawning)
            {
                Spawn();
            }
            yield return new WaitForSeconds(SpawningTime);
        }
    }
    public Particle Spawn()
    {
        if (RunTimeParticlePrefab == null) { Debug.LogError("Set RunTimeParticlePrefab!"); return null;  }
        Particle particle = RunTimeParticlePrefab.Clone();
        return Spawn(particle);
    }
    public Particle Spawn(Particle particle)
    {
        float x, z;
        GetRandomPos(SpawnArea, out x, out z);
        return Spawn(particle,new Vector3(x,1,z));
    }
    public Particle Spawn(Particle particle,Vector3 position)
    {
        return Instantiate(particle, position, Quaternion.identity, Parent);
    }
    public void Spawn(int ParticleCount)
    {
        for (int i = 0; i < ParticleCount; i++)
        {
            Spawn();
        }
    }
    private void GetRandomPos(Area area,out float x,out float z)
    {
        x = UnityEngine.Random.Range(area.LeftUpCorner.localPosition.x, area.RightDownCorner.localPosition.x);
        z = UnityEngine.Random.Range(area.RightDownCorner.localPosition.z, area.LeftUpCorner.localPosition.z);
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
