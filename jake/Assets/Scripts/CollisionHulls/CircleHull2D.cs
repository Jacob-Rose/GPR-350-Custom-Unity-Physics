using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHull2D : CollisionHull2D
{
    public float radius;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(position, radius);
    }

    public override bool detectCollision(CollisionHull2D other)
    {
        if (other is CircleHull2D)
        {
            return detectCollision(this, other as CircleHull2D);
        }
        else if (other is AABBHull)
        {
            return detectCollision(this, other as AABBHull);
        }
        else if(other is OBBHull)
        {
            return detectCollision(this, other as OBBHull);
        }
        throw new System.Exception();
    }
}
