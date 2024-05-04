using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
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
