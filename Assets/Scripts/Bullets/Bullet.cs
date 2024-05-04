using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Direction;
    //[SerializeField]
    //int Damage;
    [SerializeField]
    public int LifeTime=5000;
    public GameObject emitter { get; private set; }//owner,player,boss,host,parentm,wallah
    Mover mover;
    public Bullet ()
    {
        Direction = GetStartingDirection();
    }
    public Bullet(Vector3 direction)
    {
       this.Direction = direction;

    }
    public Bullet(Vector3 direction,GameObject emmiter)
    {
       this.Direction = direction;
       this.emitter = emmiter;
    }
    void Start()
    {
    }
    private void Awake()
    {
        DestroyThisAfter(LifeTime);
        mover = GetComponent<Mover>();
    }
    void Update()
    {
        mover.Movement=MoveDirection();
    }
    public void SetEmitter(GameObject gameObject)
    {
        emitter = gameObject;
    }
    private async void DestroyThisAfter(int lifeTime)
    {
        await Task.Delay(lifeTime);
        Destroy(gameObject);
    }
    private Vector2 MoveDirection()
    {
        //Debug.Log($"Bullet ({gameObject.GetInstanceID()}) is Moving");
        return Direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable!=null) 
            damageable.GetHit(new HitInfo() {
                Emitter=this.emitter}
            );

        Debug.Log("Bullet collide with" + collision.ToString());
        Debug.Log("from Bullet emmitter: " + emitter.ToString());
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    private Vector3 GetStartingDirection()
    {
        return new Vector3(1f,0f);
    }
}
