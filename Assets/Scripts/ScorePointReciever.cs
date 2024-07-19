using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
public class ScorePointReciever : MonoBehaviour
{
    [SerializeField]
    public AudioClip PointRecievedSound;
    public UnityEvent<int> PointRecieved;
    AudioSource AudioSource;
    public int Points { get; private set; }
    private void Awake()
    {
        AudioSource=gameObject.AddComponent<AudioSource>();
    }
    private void Update()
    {
        AudioSource.volume = GameController.instance.RecivePointVolume;
    }
    public void RecievePoints(int Points)
    {
        this.Points += Points;
        AudioSource.PlayOneShot(PointRecievedSound);
        PointRecieved.Invoke(this.Points);
    }
}
