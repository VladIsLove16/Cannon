using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    ParticleSystem _particleSystem;
    [SerializeField]
    public AudioSource _audioSource;
    [SerializeField]
    AudioClip ShootSound;
    //public GameObject Barrel;
    [SerializeField]
    public float rotationSpeed = 1f;
    [SerializeField]
    float Radius;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        RotateToPointer();
        PlayerRotate();
    }

    private void PlayerRotate()
    {
        Vector2 direction = (PointerInput.GetPointerInputVector2() - (Vector2)transform.position).normalized;
        Vector2 position = direction * Radius;
        transform.localPosition = position;
    }

    private void RotateToPointer()
    {
        Vector2 direction = PointerInput.GetPointerInputVector2() - (Vector2) transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Shoot()
    {
        _particleSystem.Play();
        _audioSource.PlayOneShot(ShootSound);
    }
}
