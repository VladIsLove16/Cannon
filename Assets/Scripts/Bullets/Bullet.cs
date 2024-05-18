using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public Vector3 Direction;
    //[SerializeField]
    //int Damage;
    [SerializeField]
    public int LifeTime=5000;
    public Player Emitter{ get; private set; }//owner,player,boss,host,parentm,wallah
    Mover mover;
    Rigidbody rb;
    Explosion explosion;
    public Bullet ()
    {
        Direction = GetStartingDirection();
    }
    public Bullet(Vector3 direction)
    {
       this.Direction = direction;

    }
    public Bullet(Vector3 direction,Player emmiter)
    {
       this.Direction = direction;
       this.Emitter = emmiter;
    }
    private void Awake()
    {
        //DestroyThisAfter(LifeTime);
        mover = GetComponent<Mover>();
        rb = GetComponent<Rigidbody>();
        explosion = GetComponent<Explosion>();
    }
    void FixedUpdate()
    {
       if(mover!=null) mover.MoveDirection=MoveDirection();
    }
    public void AddForce(Vector3 direction,float force)
    {
        rb.AddForce(direction * force);
    }
    public void SetEmitter(Player player)
    {
        Emitter = player;
        if(explosion!=null)
            explosion.Emmiter = player;
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

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable!=null) 
            damageable.GetHit(new HitInfo() {
                Emitter=this.Emitter}
            );
    }
    private Vector3 GetStartingDirection()
    {
        return new Vector3(1f,0f);
    }
}
