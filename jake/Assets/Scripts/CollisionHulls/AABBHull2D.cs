using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBHull2D : CollisionHull2D
{
    public Vector2 halfLength;
    public void OnDrawGizmos()
    {
        Vector2 offset = Vector3.zero;
        if(!Application.isPlaying)
            offset = new Vector2(transform.position.x, transform.position.y);
        Gizmos.color = new Color(0.2f, 1.0f, 1.0f);
        Gizmos.DrawLine(position + offset + halfLength, position + offset + new Vector2(halfLength.x, -halfLength.y));
        Gizmos.DrawLine(position + offset + new Vector2(halfLength.x, -halfLength.y), position + offset - halfLength);
        Gizmos.DrawLine(position + offset - halfLength, position + offset + new Vector2(-halfLength.x, halfLength.y));
        Gizmos.DrawLine(position + offset + new Vector2(-halfLength.x, halfLength.y), position + offset + halfLength);
    }

    public override bool detectCollision(CollisionHull2D other)
    {
        if (other is CircleHull2D)
        {
            return detectCollision(other as CircleHull2D, this);
        }
        else if (other is AABBHull2D)
        {
            return detectCollision(this, other as AABBHull2D);
        }
        else if (other is OBBHull2D)
        {
            return detectCollision(other as OBBHull2D, this);
        }
        throw new System.Exception();
    }

    public override HullCollision2D detectCollisionResponse(CollisionHull2D other)
    {
        if (other is CircleHull2D)
        {
            return detectCollisionResponse(other as CircleHull2D, this);
        }
        else if (other is AABBHull2D)
        {
            return detectCollisionResponse(this, other as AABBHull2D);
        }
        else if (other is OBBHull2D)
        {
            return detectCollisionResponse(other as OBBHull2D, this);
        }
        throw new System.Exception();
    }
}