using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBHull : CollisionHull2D
{
    public Vector2 halfLength;
    public float boundingBoxRotation = 0.0f;

    public Vector2 getXNormal()
    {
        return (Quaternion.Euler(0, 0, boundingBoxRotation) * new Vector3(1, 0, 0)).normalized;
    }
    public Vector2 getYNormal()
    {
        return (Quaternion.Euler(0, 0, boundingBoxRotation) * new Vector3(0, 1, 0)).normalized;
    }

    public Vector2 getBottomLeftPos()
    {
        return Quaternion.Euler(0, 0, boundingBoxRotation) * new Vector3(position.x - halfLength.x, position.y - halfLength.y);
    }
    public Vector2 getBottomRightPos()
    {
        return Quaternion.Euler(0, 0, boundingBoxRotation) * new Vector3(position.x + halfLength.x, position.y - halfLength.y);
    }
    public Vector2 getTopLeftPos()
    {
        return Quaternion.Euler(0, 0, boundingBoxRotation) * new Vector3(position.x - halfLength.x, position.y + halfLength.y);
    }
    public Vector2 getTopRightPos()
    {
        return Quaternion.Euler(0, 0, boundingBoxRotation) * new Vector3(position.x + halfLength.x, position.y + halfLength.y);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(getBottomLeftPos(), getBottomRightPos());
        Gizmos.DrawLine(getBottomLeftPos(), getTopLeftPos());
        Gizmos.DrawLine(getTopLeftPos(), getTopRightPos());
        Gizmos.DrawLine(getTopRightPos(), getBottomRightPos());
    }

    public override bool detectCollision(CollisionHull2D other)
    {
        if (other is CircleHull2D)
        {
            return detectCollision(other as CircleHull2D, this);
        }
        else if (other is AABBHull)
        {
            return detectCollision(other as AABBHull, this);
        }
        else if (other is OBBHull)
        {
            return detectCollision(this, other as OBBHull);
        }
        throw new System.Exception();
    }
}