using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : OBBHull3D
{
    public override void OnCollision(HullCollision3D coll)
    {
        base.OnCollision(coll);
        LunarLanderGameManager.m_Instance.ResetPlayer();
    }
}
