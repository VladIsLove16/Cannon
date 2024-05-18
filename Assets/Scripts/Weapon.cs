using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
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
    private float pitch;
    private AudioSource _audioSourceRotation;
    private AudioSource _audioSourceShoot;
    [Header("Bullet")]
    [SerializeField]
    public GameObject BulletPrefab;
    public List<GameObject> AvailableBullets;
    public Transform BulletContainer;
    [Header("Shoot")]
    public Transform ShootPoint;
    public Transform ExplosionPoint;
    public float ShootForce=10f;
    public float RecoilForce = 10f;
    [Header("Barrel")]
    [SerializeField]
    public float rotationSpeedY = 5f;
    public float maxYRotation=15f;
    public float minYRotation=-45f;
    private float RotationInput;
    [Header("Debug")]
    [SerializeField]
    Player player;
    [SerializeField]
    private InputActionReference RotateAction;
    [SerializeField]
    private InputActionReference WeaponSlot1, WeaponSlot2, WeaponSlot3, WeaponSlot4;
    private Rigidbody rb; 

    private void Awake()
    {
       rb=GetComponentInParent<Rigidbody>();
        _audioSourceRotation = gameObject.AddComponent<AudioSource>();
        _audioSourceShoot = gameObject.AddComponent<AudioSource>();
        player=GetComponentInParent<Player>();
    }
    private void ChoosedSlot4(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        BulletPrefab = AvailableBullets[3] ?? BulletPrefab;
    }
    private void ChoosedSlot3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        BulletPrefab = AvailableBullets[2] ?? BulletPrefab;
    }

    private void ChoosedSlot2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        BulletPrefab = AvailableBullets[1] ?? BulletPrefab;
    }
    private void ChoosedSlot1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
       BulletPrefab = AvailableBullets[0] ?? BulletPrefab;
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
        RotationInput = RotateAction.action.ReadValue<float>();
        if (Mathf.Abs(RotationInput) > 0.1f)
        {
            transform.eulerAngles += new Vector3(RotationInput * rotationSpeedY, 0, 0);
            LimitRotation();
            PlaySound();
        }

    }

    private void PlaySound()
    {
        _audioSourceRotation.pitch = RotationInput * pitch;
        float angle = Mathf.Round(transform.localEulerAngles.x);
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
            if (angle == maxYRotation || angle == minYRotation)
                _audioSourceRotation.Pause();
            if(Mathf.Abs( RotationInput)<0.1)
                _audioSourceRotation.Pause();
        }
    }
    private void LimitRotation()
    {
        Vector3 Euler = transform.rotation.eulerAngles;
        if (Euler.x > 180)
            Euler.x -= 360;
        Euler.x = Math.Clamp(Euler.x, minYRotation, maxYRotation);
        transform.rotation=Quaternion.Euler(Euler);
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

    public void Shoot()
    {
        //_particleSystem.Play();
        _audioSourceShoot.PlayOneShot(ShootSound);
        Bullet bullet = Instantiate(BulletPrefab, ShootPoint.position, transform.localRotation, BulletContainer).GetComponent<Bullet>();
        bullet.SetEmitter(player);
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*ShootForce);
        AddRecoilForce();

    }

    private void AddRecoilForce()
    {
        rb.AddForce(-transform.forward * RecoilForce,ForceMode.Acceleration);
    }
}
