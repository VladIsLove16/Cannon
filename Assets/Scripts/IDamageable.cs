using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void GetHit();
    public void GetHit(HitInfo hitInfo);
}
