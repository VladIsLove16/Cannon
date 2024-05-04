using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance = null;
    public UnityEvent Death;
    [SerializeField]
    public bool IsInvulnerable = false;
    private Vector2 pointerInput, movementInput;
    private Mover mover;
    [SerializeField]
    public Bullet BulletPrefab;
    [SerializeField]
    InputActionReference movement,attack,followCursor,spawn;
    [SerializeField]
    public Particle ObjectToInstantiate;
    [SerializeField]
    public ParticleCreator creator;
    ParticleSystem ShootParticles;
    [SerializeField]
    Weapon Weapon;
    [SerializeField]
    Transform BulletContainer;

    private void Start()
    {
        if (Instance == null)
        { // Ёкземпл€р менеджера был найден
            Instance = this; // «адаем ссылку на экземпл€р объекта
        }
        else if (Instance == this)
        { // Ёкземпл€р объекта уже существует на сцене
            Destroy(gameObject); // ”дал€ем объект
        }
    }
    private void Awake()
    {
        mover=GetComponent<Mover>();
        ShootParticles=GetComponent<ParticleSystem>();
    }
    Vector2? target;
    private float maxHoldTime = 0.1f;
    float currentHoldTime = 0;
    private void Update()
    {
        pointerInput = GetPointerInput();
        movementInput = movement.action.ReadValue<Vector2>();
        mover.Movement = Move();
    }
    private void OnEnable()
    {
        followCursor.action.performed += FollowCursor_Action_Performed;
        followCursor.action.canceled += FollowCursor_Action_Canceled;
        followCursor.action.started += FollowCursor_Action_Started;
        attack.action.performed += Attack;
        spawn.action.performed += Spawn;

    }
    private void OnDisable()
    {
        followCursor.action.performed -= FollowCursor_Action_Performed;
        followCursor.action.canceled -= FollowCursor_Action_Canceled;
        followCursor.action.started -= FollowCursor_Action_Started;
        attack.action.performed -= Attack;
        spawn.action.performed -= Spawn; 
    }
    private void Attack(InputAction.CallbackContext context)
    {
        BulletPrefab.Direction = (Vector3.MoveTowards(transform.localPosition, pointerInput, 1f)-transform.localPosition).normalized;
        
        Debug.Log("Direction: " + BulletPrefab.Direction.ToString());
        Debug.Log("Emitter: " + BulletPrefab.emitter);

        Bullet bullet = Instantiate(BulletPrefab, Weapon.transform.position, transform.localRotation, BulletContainer);
        bullet.SetEmitter(gameObject);
        Weapon.Shoot();
    }
    private void Spawn(InputAction.CallbackContext context)
    {
        if (ObjectToInstantiate == null) { Debug.Log("”кажите у Player ссылку на объект в Inspector"); return; }
        if (creator == null)
        {
            Debug.Log("”кажите у Player ссылку на спавнер в Inspector");
            Instantiate(ObjectToInstantiate, pointerInput, transform.rotation);
        }
        else
        {
            creator.Spawn(ObjectToInstantiate, pointerInput);
        }

    }
    private Vector2 Move()
    {
        if (movementInput == Vector2.zero)
            return FollowTargetVector();
        else 
            target = null;
        return movementInput;
    }
    private Vector2 FollowTargetVector()
    {
        if (target == transform.localPosition)
            target = null;
        if (target == null)
            return Vector2.zero;
        Vector3 newPos = Vector3.MoveTowards(transform.position, (Vector2)target, 1f);
        Vector3 offset = newPos - transform.position;
        return offset;
    }
    private Vector2 GetPointerInput()
    {
       return PointerInput.GetPointerInput();
    }
    private void Die()
    {
        Debug.Log("Die");
        Death.Invoke();
        Destroy(gameObject);
    }
    public void GetHit()
    {
        if (IsInvulnerable) return;
        Die();
    }
    public void GetHit(HitInfo hitInfo)
    {
        GetHit();
    }
    private void FollowCursor_Action_Started(InputAction.CallbackContext context)
    {
        target = pointerInput;
        currentHoldTime += Time.deltaTime;
    }

    private void FollowCursor_Action_Canceled(InputAction.CallbackContext context)
    {
        if (currentHoldTime > maxHoldTime)
            target = null;
        currentHoldTime = 0;
    }

    private void FollowCursor_Action_Performed(InputAction.CallbackContext context)
    {
        target = pointerInput;
    }

    
}
