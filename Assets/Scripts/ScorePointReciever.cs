using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScorePointReciever : MonoBehaviour
{
    [SerializeField]
    AudioClip PointRecievedSound;
    [SerializeField]
    AudioSource AudioSource;
    public int Points;
    public void RecievePoints(int Points)
    {
        this.Points += Points;
        AudioSource.PlayOneShot(PointRecievedSound);
    }
    
}
