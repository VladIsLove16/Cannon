using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FastEnemyParticle : Particle, IDamageable
{
    public FastEnemyParticle()
    {
       
    }
    private new void Start()
    {
        base.Start();
        // DieAsync();
    }
    public new void Update()
    {
        
    }

    public new void GetHit()
    {
        AudioSource.PlayOneShot(DeathSound);
        Destroy(gameObject);
    }
    protected new async void DieAsync()
    {
        System.Random random = new System.Random();
        await Task.Delay(random.Next(3000, 6000));
        GetHit();
    }
}
