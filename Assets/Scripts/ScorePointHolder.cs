using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScorePointHolder : MonoBehaviour
{
    [SerializeField]
    int points;
    
    public int GetPoints()
    {
        return points;
    }
}
