using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScorePointReciever : MonoBehaviour
{
    [SerializeField]
    public AudioClip PointRecievedSound;
    AudioSource AudioSource;
    public int Points;
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
    }
}
