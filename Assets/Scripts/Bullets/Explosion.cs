using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ExplodeSound;
    public int DestroyGODelay;
    public float Force;
    public float Radius;
    public float ExplodeChainRadius;
    public bool explodeNearExplosions=true;
    public bool exploded = false;
    public bool Activate = false;
    protected ParticleSystem particleSystem;
    protected Rigidbody rb;
    protected MeshRenderer mr;
    protected Explosion Exploder;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        particleSystem= GetComponent<ParticleSystem>();
        rb= GetComponentInParent<Rigidbody>();
        mr= GetComponentInParent<MeshRenderer>();
    }
    private void Update()
    {
        if (Activate)
            Explode(this);
    }
    public void Explode(Explosion Exploder)
    {
        if (exploded) return;
        exploded=true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.attachedRigidbody;
            if(rb != null)
            {
                rb.AddExplosionForce(Force, transform.position, Radius);
            }
            ExplodeCollider(collider);
        }
        audioSource.clip = ExplodeSound;
        audioSource.Play();
        rb.isKinematic = true;
        mr.enabled = false;
        particleSystem.Play();
        Destroy();
    }

    private async void Destroy()
    {
        
        await Task.Delay(DestroyGODelay);
        audioSource.Stop();
        Destroy(gameObject);
    }

    private void ExplodeCollider(Collider collider)
    {
        if (!explodeNearExplosions) return;
        Explosion explosion = collider.GetComponent<Explosion>();
        if(explosion==null)
            explosion = collider.GetComponentInChildren<Explosion>();
        if (explosion != null)
        {
            explosion.Explode(this);
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplodeChainRadius);

    }
}
