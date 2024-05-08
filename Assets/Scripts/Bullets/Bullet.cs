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
    public Player emitter = null;//owner,player,boss,host,parentm,wallah
    Mover mover;
    Rigidbody rb;
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
       this.emitter = emmiter;
    }
    void Start()
    {
    }
    private void Awake()
    {
        //DestroyThisAfter(LifeTime);
        mover = GetComponent<Mover>();
        rb= GetComponent<Rigidbody>();  
    }
    void Update()
    {
       if(mover!=null) mover.MoveDirection=MoveDirection();
    }
    public void AddForce(Vector3 direction,float force)
    {
        rb.AddForce(direction * force);
    }
    public void SetEmitter(Player player)
    {
        emitter = player;
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
