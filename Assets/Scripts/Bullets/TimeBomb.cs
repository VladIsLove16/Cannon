using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBomb : Explosion
{
    public AudioClip ExplodingSound;
    public bool Exploding;
    public float DefaultDelay = 0.5f;
    public float TimeToExplode;
    private Renderer renderer;
    private void Awake()
    {

        renderer = GetComponent<Renderer>();
    }
    private void Update()
    {
        if (!exploded)
        {
            if (Exploding)
            {
                TimeToExplode -= Time.deltaTime;
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = ExplodingSound;
                    audioSource.Play();
                }
                renderer.material.color = Color.red;
            }
            else
            {
                if (audioSource.isPlaying)
                    audioSource.Pause();
                renderer.material.color = Color.yellow;
            }
            if (TimeToExplode <= 0) base.Explode(this);
        }
    }
    public new void Explode(Explosion exploder)
    {
        float distanceToexploder = (transform.position - exploder.gameObject.transform.position).magnitude;
        StartExploding(DefaultDelay * (distanceToexploder / ExplodeChainRadius)); 
    }
    private void StartExploding(float delay)
    {
        Exploding = true;
        TimeToExplode = Mathf.Min(delay, TimeToExplode);
        GetComponent<Renderer>().material.color = Color.red;
    }
}
