using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    private float defaultTimeScale;
    public TextMeshProUGUI PauseButtonText;
    [SerializeField]
    public Transform EnemyContainer;
    public Player Player;
    public TextMeshProUGUI PlayerInvulnerableButtonText;
    public float SpeedChangeValue;
    public float ScaleChangeValue;
    public void Awake()
    {
        defaultTimeScale=Time.timeScale;
        PlayerInvulnerableButtonText.text="Inv: "+ Player.IsInvulnerable.ToString();
    }
    public void Pause()
    {
        Time.timeScale = 0;
        PauseButtonText.text = "Continue";
    }
    public void Continue()
    {
        Time.timeScale=defaultTimeScale;
        PauseButtonText.text = "Pause";
    }
    public void PauseOrContinue()
    {
        if (Time.timeScale == 0)
            Continue();
        else
            Pause();
    }
    public void EnemySizeUp()
    {
        foreach (Transform child in EnemyContainer)
        {
            child.localScale = new Vector3((float)(child.localScale.x + ScaleChangeValue), (float)(child.localScale.y + ScaleChangeValue));
        }
    }
    public void EnemySizeDown()
    {
        foreach (Transform child in EnemyContainer)
        {
            child.localScale = new Vector3((float)(child.localScale.x - ScaleChangeValue), (float)(child.localScale.y - ScaleChangeValue));
        }
    }
    public void EnemySpeedUp()
    {
        foreach (Transform child in EnemyContainer)
        {
            child.GetComponent<Particle>().movementSpeed +=SpeedChangeValue;
        }
    }
    public void EnemySpeedDown()
    {
        foreach (Transform child in EnemyContainer)
        {
            child.GetComponent<Particle>().movementSpeed -= SpeedChangeValue;
        }
    }
    public void SwitchInvulnerable()
    {
        if (Player.IsInvulnerable)
        { 
            Player.IsInvulnerable = false;
            PlayerInvulnerableButtonText.text = "Inv: " + Player.IsInvulnerable;
        }
        else
        {
            Player.IsInvulnerable = true;
            PlayerInvulnerableButtonText.text = "Inv: " + Player.IsInvulnerable;
        }
    }

}
