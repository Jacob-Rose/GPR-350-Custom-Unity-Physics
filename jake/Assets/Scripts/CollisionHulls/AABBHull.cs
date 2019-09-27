using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBHull : CollisionHull2D
{
    public Vector2 halfLength;
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(position, new Vector3(halfLength.x * 2.0f, halfLength.y * 2.0f, 2.0f));
    }

    public override bool detectCollision(CollisionHull2D other)
    {
        if (other is CircleHull2D)
        {
            return detectCollision(other as CircleHull2D, this);
        }
        else if (other is AABBHull)
        {
            return detectCollision(this, other as AABBHull);
        }
        else if (other is OBBHull)
        {
            return detectCollision(this, other as OBBHull);
        }
        throw new System.Exception();
    }
}