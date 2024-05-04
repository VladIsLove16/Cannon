using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    LoseScreen LoseScreen;
    [SerializeField]
    AudioClip LoseClip;
    [SerializeField]
    AudioClip BackgroundMusic;
    [SerializeField]
    AudioSource AudioSource; 
    [SerializeField]
    Scenes Scenes;
    private void Start()
    {
        Player.Instance.Death.AddListener(Lose);
        AudioSource.clip = BackgroundMusic;
        AudioSource.Play();
    }
   private void Lose()
   {
        LoseScreen.Setup(Player.Instance.GetComponent<ScorePointReciever>().Points);
        if(AudioSource.isPlaying) { AudioSource.Stop(); }
        AudioSource.PlayOneShot(LoseClip);
        Debug.Log("Lost");
    }

}
