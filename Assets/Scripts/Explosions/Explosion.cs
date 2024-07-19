using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Explosion : Hitter, IDamageable
{
    
    public AudioClip ExplodeSound;
    public float Force;
    public float Radius;
    public float ExplodeChainRadius;
    public bool ExplodeNearExplosions=true;
    public bool Activate = false; 
    public bool DestroyOnExplode = false;
    public bool exploded = false;
    [HideInInspector]
    public Player Emmiter;
    protected int DestroyGODelay;
    protected new ParticleSystem particleSystem;
    protected Rigidbody rb;
    protected MeshRenderer mr;
    protected new Collider collider;
    protected AudioSource audioSource;
    protected ScorePointHolder scorePointHolder;
    [SerializeField]
    private int damage;
    public int Damage { get => damage; set => damage=value; }

    protected virtual void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        rb= GetComponentInParent<Rigidbody>();
        mr= GetComponentInParent<MeshRenderer>();
        collider = GetComponent<Collider>();
        scorePointHolder = GetComponent<ScorePointHolder>();
    }
    protected virtual void Update()
    {
        audioSource.volume = GameController.instance.ExplosionVolume;
        if (Activate)
        {
            exploded=false;
            Explode(new HitInfo(this, Emmiter));
            Activate = false; 
        }
    }
    public virtual void Explode(HitInfo hitInfo)
    {
        if (exploded) return;
        exploded=true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider collider in colliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.GetHit(new HitInfo(this, hitInfo.Emitter));
        }
        colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(Force, transform.position, Radius,2f);
            }
            //ExplodeNearExplosion(collider,hitInfo);
        }
        RecievePoints(hitInfo.Emitter);
        audioSource.clip = ExplodeSound;
        audioSource.Play();
        if(particleSystem!=null)
            particleSystem.Play();
        if(mr!=null)
            mr.enabled = false;
        if(collider!=null)
            collider.enabled = false;
        rb.isKinematic = true;
        if (DestroyOnExplode)
           StartCoroutine( Destroy(Convert.ToInt32(audioSource.clip.length*1000)));
    }

    private void RecievePoints(Player player)//в gameController перенести 1000000%%!!!!!
    {
        if (player != null)
        {
            ScorePointReciever scorePointReciever = player.GetComponent<ScorePointReciever>();
            if (scorePointReciever != null)
                if (scorePointHolder != null)
                    scorePointReciever.RecievePoints(scorePointHolder.Points);
        }
    }

    //private void ExplodeNearExplosion(HitInfo hitInfo)
    //{
    //    if (ExplodeNearExplosions)
    //    {
    //        Explosion explosion = collider.GetComponent<Explosion>();
    //        if (explosion == null)
    //            explosion = collider.GetComponentInChildren<Explosion>();
    //        if (explosion != null)
    //        {
    //            explosion.Explode(hitInfo, this);
    //        }
    //    }
    //}

    private IEnumerator Destroy(int Delay)
    {
        yield return new WaitForSeconds(Delay);
        Destroy();
    }
    private void Destroy()
    {
        if(gameObject!=null)
            Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplodeChainRadius);

    }

    public void GetHit()
    {
        Explode(new HitInfo(this, Emmiter));
    }

    public void GetHit(HitInfo hitInfo)
    {
        Explode(hitInfo);
    }
}
