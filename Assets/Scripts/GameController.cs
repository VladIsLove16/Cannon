using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField]
    LoseScreen LoseScreen;
    [SerializeField]
    AudioClip LoseClip;
    [SerializeField]
    AudioClip WinClip;
    AudioSource AudioSource; 
    [SerializeField]
    Scenes Scenes;
    [SerializeField]
    UI ui;
    [SerializeField]
    InputActionReference pause;
    public bool isPaused;
    public Player player;
    public float MusicVolume;
    public float ExplosionVolume;
    public float RecivePointVolume;
    public float ShootVolume;
    public UnityEvent<float> RemainingTimeChanged;
    public float RemainingTime;
    [SerializeField]
    public int ScoreToWin;
    private bool win;
    private void Awake()
    {
        player.Death.AddListener(Lose);
        AudioSource = gameObject.GetComponent<AudioSource>();
        if(instance == null)
            instance = this;
        player.GetComponent<ScorePointReciever>().PointRecieved.AddListener(WinController);
        pause.action.performed += Pause;
    }

    private void WinController(int score)
    {
        if(score >= ScoreToWin && !win)
            Win();
    }
    private void FixedUpdate()
    {
        TimeRunOut();
    }
    private void TimeRunOut()
    {
        RemainingTime -= Time.deltaTime;
        RemainingTimeChanged?.Invoke(RemainingTime);
        if (RemainingTime <= 0 && !win)
            Lose();

    }
    private void Win()
    {
        win = true;
        ui.OnWin();
        AudioSource.PlayOneShot(WinClip);
    }
    private void Lose()
    {
        if(LoseScreen!=null) LoseScreen.Setup(player.GetComponent<ScorePointReciever>().Points); 
        if(AudioSource.isPlaying) { AudioSource.Stop(); }
        AudioSource.PlayOneShot(LoseClip);
        ui.OnLose();
        Debug.Log("Lost");
    }

    public void Play()
    {
        Time.timeScale = 1f;
        ui.SetMenuEnabled(false);
        isPaused = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
    public void Pause(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(isPaused)
        {
            Play();
        }
        else {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            Time.timeScale = 0f;
            ui.SetMenuEnabled(true);
            isPaused = true;
        }
    }
    public void Exit()
    {
        Debug.Log("Exit");
    }
}
