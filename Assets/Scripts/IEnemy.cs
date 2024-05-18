using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Player player= collision.transform.GetComponent<Player>();
        if(player != null )
        {
            player.GetHit();
        }
    }
}
