using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class Particle : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected int hp = 1;
    [HideInInspector]
    protected AudioSource AudioSource;
    [SerializeField]
    protected AudioClip GetHitSound;
    [SerializeField]
    protected AudioClip DeathSound;
    protected Vector3 lastDirection;
    [SerializeField]
    public IBounce BounceAction;

    protected new ParticleSystem particleSystem;
    protected Rigidbody rb;
    protected MeshRenderer mr;
    protected new Collider collider;
    protected bool Diyengl;
    public Particle()
    {
        BounceAction=new ExplodeBounce();
        lastDirection = GetStartingDirectionv1();
    }
    public void Start()
    {
    }
    public void Update()
    {
        
    }
    public void Awake()
    {
        AudioSource=GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        rb = GetComponentInParent<Rigidbody>();
        mr = GetComponentInParent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }
    public Particle Clone()
    {
        return this;
    }
    private Vector3 GetStartingDirectionv1()   
    {
       return new Vector3(1f, 1f);
    } 
    private Vector3 GetStartingDirectionv2()   
    {
       return new Vector3(-1f, 1f);
    }
    
    protected void Bounce(Collision2D collision)
    {
        Vector3 direction = Vector3.Reflect(lastDirection.normalized, collision.contacts[0].normal);
        lastDirection = direction;
        BounceAction.Execute(transform);
    }

    public void GetHit()
    {
        hp--;
        
        if (hp <= 0)
        { 
            Die();
        }
        else AudioSource.PlayOneShot(GetHitSound);
    }

    private async void Die()
    {
        if (Diyengl) return;
        Diyengl = true;
        AudioSource.PlayOneShot(DeathSound);
        if (particleSystem != null)
            particleSystem.Play();
        if (mr != null)
            mr.enabled = false;
        if (collider != null)
            collider.enabled = false;
        rb.isKinematic = true;
        await Task.Delay(Mathf.RoundToInt(GetHitSound.length*1000));
        Destroy(gameObject);
    }

    public void GetHit(HitInfo hitInfo)
    {
        GetHit();
        if(hitInfo.Emitter!=null)
            Debug.Assert(true,"Pritcle Getting Hitted From : "+hitInfo.Emitter.ToString());
    }
    protected async void DieAsync()
    {
        System.Random random = new System.Random();
            
        GetHit();
    }
}
