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
    [SerializeField] 
    protected AudioSource AudioSource;
    [SerializeField]
    protected AudioClip GetHitSound;
    [SerializeField]
    protected AudioClip DeathSound;
    [SerializeField]
    public float movementSpeed;
    protected Vector3 lastDirection;
    [SerializeField]
    public IBounce BounceAction;
    ScorePointHolder ScorePointHolder;
    public Particle()
    {
        BounceAction=new ExplodeBounce();
        lastDirection = GetStartingDirectionv1();
    }
    public void Start()
    {
    }
    private void Awake()
    {
        AudioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        ScorePointHolder= gameObject.GetComponent<ScorePointHolder>();
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
    public void Update()
    { 
        Move();

    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Bounce(collision); 
        }
    }
    protected void Bounce(Collision2D collision)
    {
        Vector3 direction = Vector3.Reflect(lastDirection.normalized, collision.contacts[0].normal);
        lastDirection = direction;
        BounceAction.Execute(transform);
    }
    protected void Move()
    {
        float dx = movementSpeed * lastDirection.x;
        float dy = movementSpeed * lastDirection.y;
        Vector2 newPos = transform.localPosition + new Vector3(movementSpeed * Time.deltaTime * dx, movementSpeed * Time.deltaTime * dy, 0);
        transform.localPosition = newPos;
        //Debug.Log($"Particle ({gameObject.GetInstanceID()}) is Moving");
    }

    public void GetHit()
    {
        hp--;
        if(hp <= 0)
        { 
            Destroy(gameObject);
            AudioSource.PlayOneShot(GetHitSound);
        }    
    }
    public void GetHit(HitInfo hitInfo)
    {
        GetHit();
        Debug.Assert(true,"From bullet, emitter: "+hitInfo.Emitter.ToString());
        ScorePointReciever scorePointReciever = hitInfo.Emitter.GetComponent<ScorePointReciever>();
        if(scorePointReciever != null ) { scorePointReciever.RecievePoints(ScorePointHolder.GetPoints()); }
    }
    protected async void DieAsync()
    {
        System.Random random = new System.Random();
            
        GetHit();
    }
}
