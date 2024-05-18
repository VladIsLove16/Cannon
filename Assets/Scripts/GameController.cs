using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField]
    LoseScreen LoseScreen;
    [SerializeField]
    AudioClip LoseClip;
    AudioSource AudioSource; 
    [SerializeField]
    Scenes Scenes;
    public Player player;
    public float MusicVolume;
    public float ExplosionVolume;
    public float RecivePointVolume;
    public float ShootVolume;

    private void Start()
    {
        player.Death.AddListener(Lose);
        AudioSource = gameObject.GetComponent<AudioSource>();
        if(instance == null)
            instance = this;    
    }
   private void Lose()
   {
        LoseScreen.Setup(player.GetComponent<ScorePointReciever>().Points); 
        if(AudioSource.isPlaying) { AudioSource.Stop(); }
        AudioSource.PlayOneShot(LoseClip);
        Debug.Log("Lost");
    }

}
