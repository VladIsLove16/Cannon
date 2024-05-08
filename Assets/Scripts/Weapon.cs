using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public AudioSource _audioSource;
    [SerializeField]
    AudioClip ShootSound;
    //public GameObject Barrel;
    [SerializeField]
    public float rotationSpeedY = 5f;
    [SerializeField]
    public Bullet BulletPrefab;
    public Transform ShootPoint;
    public Transform ExplosionPoint;
    public Transform BulletContainer;
    public float ShootTime;
    public float ShootForce=10f;
    public float RecoilFore = 10f;
    public float ExplosionRadius = 2f;
    public float maxYRotation=15f;
    public float minYRotation=-45f;
    [SerializeField]
    InputActionReference Rotate;
    public float Rotation;
    Rigidbody rb;
    private void Awake()
    {
       rb=GetComponentInParent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Rotation = Rotate.action.ReadValue<float>();
        if(Mathf.Abs( Rotation) > 0.1f )
        {
            transform.eulerAngles += new Vector3(Rotation * rotationSpeedY, 0, 0);
            LimitRotation();
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
        _audioSource.PlayOneShot(ShootSound);
        Bullet bullet = Instantiate(BulletPrefab, ShootPoint.position, transform.localRotation, BulletContainer);
        Debug.Log("Weapon Forward Vector: " + transform.forward);
        bullet.SetEmitter(GetComponentInParent<Player>());
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*ShootForce);
        AddRecoilForce();

    }

    private void AddRecoilForce()
    {
        rb.AddForce(-transform.forward * RecoilFore,ForceMode.Acceleration);
    }
}
