using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void LoadScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    public void LoadScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
