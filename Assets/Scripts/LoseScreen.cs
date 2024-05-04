using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    public TextMeshProUGUI Score;
    public void Setup(int points)
    {
        Score.text="Score: "+points.ToString();
        gameObject.SetActive(true);
    }
}
