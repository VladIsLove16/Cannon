using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoyable : MonoBehaviour
{
    [SerializeField]
    GameObject cellsPrefab;
    [SerializeField]
    GameObject original;
    private void Awake()
    {
        //cellsPrefab.SetActive(false);
    }
    public void DestroyThis()
    {
        original.SetActive(false);
        cellsPrefab.SetActive(true);
    }
}
