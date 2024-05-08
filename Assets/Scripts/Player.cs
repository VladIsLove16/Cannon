using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private Vector2 pointerInput;
    [SerializeField]
    InputActionReference attack,followCursor,spawn;
    [SerializeField]
    public Particle ObjectToInstantiate;
    [SerializeField]
    public ParticleCreator creator;
    [SerializeField]
    Weapon Weapon;
    Mover mover;
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
        mover = GetComponent<Mover>();
    }
    Transform target
    {
        get
        {
            return mover.Target;
        }
        set
        {
            mover.Target = value;
        }
    }
    private float maxHoldTime = 0.1f;
    float currentHoldTime = 0;
    private void Update()
    {
        pointerInput = GetPointerInput();
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
    private void Attack(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Weapon.Shoot();
    }
    private void Spawn(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        
        if (ObjectToInstantiate == null) { Debug.Log("”кажите у Player ссылку на объект в Inspector"); return; }
        if (creator == null)
        {
            Instantiate(ObjectToInstantiate, transform.position+(transform.forward*3f), transform.rotation);
        }
        else
        {
            creator.Spawn(ObjectToInstantiate, pointerInput);
        }

    }
    private Vector3 GetPointerInput()
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
    private void FollowCursor_Action_Started(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        target.position = pointerInput;
        currentHoldTime += Time.deltaTime;
    }

    private void FollowCursor_Action_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (currentHoldTime > maxHoldTime)
            target = null;
        currentHoldTime = 0;
    }

    private void FollowCursor_Action_Performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        target.position = pointerInput;
    }

    
}
