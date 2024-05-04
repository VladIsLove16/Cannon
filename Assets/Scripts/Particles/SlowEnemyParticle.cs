using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SlowEnemyParticle : Particle, IEnemy, IDamageable
{
    public SlowEnemyParticle()
    {
        movementSpeed = 1;
        hp = 2;
    }
    private new void Start()
    {
        base.Start();
       // DieAsync();
    }
    public new void Update()
    {
        base.Update();  
    }
    public new void GetHit()
    {
        hp--;
        if (hp <= 0)
        { 
            AudioSource.PlayOneShot(DeathSound);
            Destroy(gameObject);
        }
        else AudioSource.PlayOneShot(GetHitSound);
    }
    protected new async void DieAsync()
    {
        System.Random random = new System.Random();
        await Task.Delay(random.Next(3000, 6000));
        GetHit();
    }
}
