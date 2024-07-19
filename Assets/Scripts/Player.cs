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
public class Player : MonoBehaviour, IDamageable, IEmmiter
{
    public UnityEvent Death;
    [SerializeField]
    public bool IsInvulnerable = false;
    private Vector2 pointerInput;
    [SerializeField]
    InputActionReference attack,spawn;
    [SerializeField]
    public Particle ObjectToInstantiate;
    [SerializeField]
    public ParticleCreator creator;
    [SerializeField]
    Weapon Weapon;
    Mover mover;
    AudioSource AudioSource;
    private void Start()
    {
    }
    private void Awake()
    {
        mover = GetComponent<Mover>();
        AudioSource=GetComponent<AudioSource>();
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
        attack.action.performed += Attack;
        spawn.action.performed += Spawn;

    }
    private void OnDisable()
    {
        attack.action.performed -= Attack;
        spawn.action.performed -= Spawn; 
    }
    private void Attack(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(!GameController.instance.isPaused)
        Emmit();
    }
    private void Spawn(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!GameController.instance.isPaused) { 
        if (ObjectToInstantiate == null) { Debug.LogError("”кажите у Player ссылку на объект в Inspector"); return; }
        if (creator == null)
        {
            Instantiate(ObjectToInstantiate, transform.position+(transform.forward*3f), transform.rotation);
        }
        else
        {
            creator.Spawn(ObjectToInstantiate, pointerInput);
        }
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

    public void Emmit()
    {
        Weapon.Shoot(this);
    }
}
