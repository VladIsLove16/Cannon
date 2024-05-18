using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBombExplosion : Explosion
{
    [Header("TimeBomb")]
    protected AudioSource TimeBombExplosionAudioSource;
    public AudioClip ExplodingSound;
    public bool Exploding;
    [SerializeField]
    public float ExplodeDelay;
    public float CurrentTimeToExplode=10000;
    [SerializeField]
    public Material ExplodingMaterial;
    [SerializeField]
    public Material NormalMaterial;
    protected override void Awake()
    {
        base.Awake();
        TimeBombExplosionAudioSource = gameObject.AddComponent<AudioSource>();
    }
    protected override void Update()
    {
        base.Update();
        if (!exploded)
        {
            if (Exploding)
            {
                CurrentTimeToExplode -= Time.deltaTime;
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = ExplodingSound;
                    audioSource.Play();
                }
                mr.material= ExplodingMaterial;
                if (CurrentTimeToExplode <= 0) base.Explode(Emmiter, this);
            }
            else
            {
                if (audioSource.isPlaying)
                    audioSource.Pause();
                mr.material = NormalMaterial;
            }
            
        }
    }
    public override void Explode(Player Emmitter, Explosion exploder)
    {
        if (Exploding) return;
        Debug.Log("TimeBombExplosion");
        float distanceToexploder = (transform.position - exploder.gameObject.transform.position).magnitude;
        StartExploding(ExplodeDelay * (distanceToexploder / ExplodeChainRadius)+2f); 
    }
    private void StartExploding(float delay)
    {
        Exploding = true;
        CurrentTimeToExplode = Mathf.Min(delay, CurrentTimeToExplode);

        Debug.Log(CurrentTimeToExplode);
        GetComponent<Renderer>().material.color = Color.red;
    }
}
