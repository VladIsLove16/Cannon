using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScorePointHolder : MonoBehaviour
{
    public int Points;
    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet =  collision.gameObject.GetComponent<Bullet>();
        if ((bullet!=null))
        {
            Player player = bullet.Emitter;
            if (player != null)
            {
                ScorePointReciever scorePointReciever = player.GetComponent<ScorePointReciever>();
                if (scorePointReciever != null)
                {
                    scorePointReciever.RecievePoints(Points);
                    Destroy(gameObject);
                }
            }
        }
    }
}
