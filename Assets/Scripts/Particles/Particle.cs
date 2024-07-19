using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class Particle : MonoBehaviour, IDamageable
{
    [SerializeField]
    public int Points;
    [SerializeField]
    protected int hp = 1;
    [HideInInspector]
    protected AudioSource AudioSource;
    [SerializeField]
    protected AudioClip GetHitSound;
    [SerializeField]
    protected AudioClip DeathSound;
    protected Vector3 lastDirection;
    protected new ParticleSystem particleSystem;
    protected Rigidbody rb;
    protected Destoyable destoyable;
    [SerializeField]
    protected SkinnedMeshRenderer CatExceptionRenderer;
    protected MeshRenderer  mr;

    protected new Collider collider;
    protected bool Diyeng;
    public Particle()
    {
        lastDirection = GetStartingDirectionv1();
    }
    public void Start()
    {
    }
    public void Update()
    {
        AudioSource.volume=GameController.instance.ExplosionVolume;
    }
    public void Awake()
    {
        AudioSource=GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        destoyable= GetComponent<Destoyable>();
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
    public void GetHit()
    {
        Debug.Log("GetHited");
        hp--;
        
        if (hp <= 0)
        { 
           StartCoroutine( Die());
        }
        else AudioSource.PlayOneShot(GetHitSound);
    }

    private IEnumerator Die()
    {
        if (Diyeng) yield break;
        Diyeng = true;
        AudioSource.PlayOneShot(DeathSound);
        if (particleSystem != null)
            particleSystem.Play();
        if (mr != null)
            mr.enabled = false;
        if (CatExceptionRenderer != null)
            CatExceptionRenderer.enabled = false;
        if (collider != null)
            collider.enabled = false;
        rb.isKinematic = true;
        yield return new WaitForSeconds(DeathSound.length);
        if(gameObject!=null)
            Destroy(gameObject);
    }

    public void GetHit(HitInfo hitInfo)
    {
        GetHit();
        destoyable.DestroyThis();
        hitInfo.Emitter?.GetComponent<ScorePointReciever>().RecievePoints(Points); 
        if (hitInfo.Emitter!=null)
            Debug.Assert(true,"Pritcle Getting Hitted From : "+hitInfo.Emitter.ToString());
    }
    protected async void DieAsync()
    {
        System.Random random = new System.Random();
            
        GetHit();
    }
}
