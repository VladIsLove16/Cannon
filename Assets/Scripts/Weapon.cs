using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    AudioClip ShootSound;
    [SerializeField]
    AudioClip RotationSound;
    [SerializeField]
    public AudioClip WeaponSlotSwitch;
    [SerializeField]
    private float pitch;
    private AudioSource _audioSourceRotation;
    private AudioSource _audioSourceShoot;
    private AudioSource _audioSourceWeaponSwitch;
    [Header("Bullet")]
    [SerializeField]
    public GameObject BulletPrefab;
    public List<GameObject> AvailableBullets;
    public Transform BulletContainer;
    public UnityEvent<int> WeaponSwitched;
    [Header("Shoot")]
    public Transform ShootPoint;
    public Transform ExplosionPoint;
    public float ShootForce=10f;
    public float RecoilForce = 10f;
    public float nextTimeToShoot;
    public float ReloadTime;
    public UnityEvent ReloadingAssert;
    [Header("Barrel")]
    [SerializeField]
    public float rotationSpeedY = 5f;
    public float maxYRotation=20f;
    public float minYRotation=-20f;
    private float RotationInput;
    public Transform Barrel;
    [Header("Debug")]
    [SerializeField]
    private InputActionReference RotateAction;
    [SerializeField]
    private InputActionReference WeaponSlot1, WeaponSlot2, WeaponSlot3, WeaponSlot4;
    private Rigidbody rb;
    private bool CanShoot;
    private void Awake()
    {
        rb=GetComponentInParent<Rigidbody>();
        _audioSourceRotation = gameObject.AddComponent<AudioSource>();
        _audioSourceShoot = gameObject.AddComponent<AudioSource>();
        _audioSourceWeaponSwitch = gameObject.AddComponent<AudioSource>();
    }
    private void ChoosedSlot4(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SetBulletSlot(3);
    }
    private void ChoosedSlot3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SetBulletSlot(2);
    }

    private void ChoosedSlot2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SetBulletSlot(1);
    }
    private void ChoosedSlot1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SetBulletSlot(0);
    }
    public void SetBulletSlot(int slot)
    {
        if (!GameController.instance.isPaused)
        {
            if (slot >= AvailableBullets.Count) { Debug.LogError(slot + " is out of " + AvailableBullets.Count); return; }
            BulletPrefab = AvailableBullets[slot] ?? BulletPrefab;
            _audioSourceWeaponSwitch.clip = WeaponSlotSwitch;
            _audioSourceWeaponSwitch.Play();
            WeaponSwitched?.Invoke(slot);
        }
    }
    private void OnEnable()
    {
        WeaponSlot1.action.performed += ChoosedSlot1;
        WeaponSlot2.action.performed += ChoosedSlot2;
        WeaponSlot3.action.performed += ChoosedSlot3;
        WeaponSlot4.action.performed += ChoosedSlot4;
    }
    private void OnDisable()
    {
        WeaponSlot1.action.performed -= ChoosedSlot1;
        WeaponSlot2.action.performed -= ChoosedSlot2;
        WeaponSlot3.action.performed -= ChoosedSlot3;
        WeaponSlot4.action.performed -= ChoosedSlot4;
    }
   

    private void Update()
    {
        _audioSourceShoot.volume = GameController.instance.ShootVolume;
    }
    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (!GameController.instance.isPaused)
        {
            RotationInput = RotateAction.action.ReadValue<float>();
            if (Mathf.Abs(RotationInput) > 0.1f)
            {
                Barrel.localEulerAngles += new Vector3(0, RotationInput * rotationSpeedY, 0);
                LimitRotation();
                PlaySound();
            }
        }
    }

    private void PlaySound()
    {
        _audioSourceRotation.pitch = RotationInput * pitch;
        float angle = Mathf.Round(Barrel.localEulerAngles.y);
        if (angle > 180) angle = angle - 360;
        if (_audioSourceRotation.clip == null)
            _audioSourceRotation.clip = RotationSound;
        if (!_audioSourceRotation.isPlaying)
        {
            if (angle != maxYRotation && angle != minYRotation)
            {
                float angleDelta = minYRotation - maxYRotation;
                float state = angle - maxYRotation;
                _audioSourceRotation.time = Mathf.Abs(state / angleDelta * _audioSourceRotation.clip.length);
                _audioSourceRotation.Play();

            }
        }
        else{
            Debug.Log("Sound play");
            if (angle == maxYRotation || angle == minYRotation)
                _audioSourceRotation.Pause();
            if(Mathf.Abs( RotationInput)<0.1)
                _audioSourceRotation.Pause();
        }
    }
    private void LimitRotation()
    {
        Vector3 Euler = Barrel.localRotation.eulerAngles;
        if (Euler.y > 180)
            Euler.y -= 360;
        Euler.y = Math.Clamp(Euler.y, minYRotation, maxYRotation);
        Barrel.localRotation =Quaternion.Euler(Euler);
    }

    //private void PlayerRotate()
    //{
    //    Vector2 direction = (PointerInput.GetPointerInputVector2() - (Vector2)transform.position).normalized;
    //    Vector2 position = direction * Radius;
    //    transform.localPosition = position;
    //}

    //private void RotateToPointer()
    //{
    //    Vector2 direction = PointerInput.GetPointerInputVector2() - (Vector2) transform.position;
    //    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Euler(0, 0, angle);
    //}

    public void Shoot(Player Emmiter)
    {
        if( nextTimeToShoot<Time.time) {
            //_particleSystem.Play();
            _audioSourceShoot.PlayOneShot(ShootSound);
            Bullet bullet = Instantiate(BulletPrefab, ShootPoint.position, transform.localRotation, BulletContainer).GetComponent<Bullet>();
            bullet.SetEmitter(Emmiter);
            Vector3 Euler = Barrel.localRotation.eulerAngles;
            if (Euler.y > 180)
                Euler.y -= 360;
            bullet.gameObject.GetComponent<Rigidbody>().AddForce((ShootPoint.position-ExplosionPoint.position) * ShootForce); ;
            Debug.Log("Barrel Angle^ " + Vector3.MoveTowards(ExplosionPoint.localPosition, ShootPoint.localPosition, 1f));
            
            Debug.Log(transform.forward);
            AddRecoilForce();
            nextTimeToShoot=Time.time+ReloadTime;
            //transform.forward+new Vector3(0,Barrel.eulerAngles.y,0)
        }
        else
        {
            ReloadingAssert?.Invoke();
            Debug.Log("Reloading!!");
        }

    }

    private void AddRecoilForce()
    {
        rb.AddForce(-transform.forward * RecoilForce,ForceMode.Force);
    }
}
