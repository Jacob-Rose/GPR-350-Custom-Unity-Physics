using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBHull2D : CollisionHull2D
{
    public Vector2 halfLength;

    public Vector2 getXNormal()
    {
        return (Quaternion.Euler(0, 0, rotation) * new Vector3(1, 0, 0)).normalized;
    }
    public Vector2 getYNormal()
    {
        return (Quaternion.Euler(0, 0, rotation) * new Vector3(0, 1, 0)).normalized;
    }

    public Vector2 getBottomLeftPos()
    {
        return (Quaternion.Euler(0, 0, rotation) * new Vector3(-halfLength.x, -halfLength.y)) + new Vector3(position.x, position.y);
    }
    public Vector2 getBottomRightPos()
    {
        return (Quaternion.Euler(0, 0, rotation) * new Vector3(halfLength.x,-halfLength.y)) + new Vector3(position.x, position.y);
    }
    public Vector2 getTopLeftPos()
    {
        return (Quaternion.Euler(0, 0, rotation) * new Vector3(-halfLength.x,halfLength.y)) + new Vector3(position.x, position.y);
    }
    public Vector2 getTopRightPos()
    {
        return (Quaternion.Euler(0, 0, rotation) * new Vector3(halfLength.x,halfLength.y)) + new Vector3(position.x, position.y);
    }

    public void OnDrawGizmos()
    {
        Vector2 offset = Vector3.zero;
        if (!Application.isPlaying)
            offset = new Vector2(transform.position.x, transform.position.y);
        Gizmos.color = drawColor;
        Gizmos.DrawLine(getBottomLeftPos() + offset, getBottomRightPos()+offset);
        Gizmos.DrawLine(getBottomLeftPos()+offset, getTopLeftPos()+offset);
        Gizmos.DrawLine(getTopLeftPos()+offset, getTopRightPos()+offset);
        Gizmos.DrawLine(getTopRightPos()+offset, getBottomRightPos()+offset);
    }

    //to find the closest point was very difficult, so i got the code from 
    //http://community.monogame.net/t/closest-point-on-obb-to-given-point/9129/3
    //I modified it to used Vector2 and the correct variable names though and understand it
    public override Vector2 GetClosestPoint(Vector2 point)
    {
        // vector from box centre to point
        Vector2 directionVector = point - position;

        // for each OBB axis...
        // ...project d onto that axis to get the distance 
        // along the axis of d from the box center
        // then if distance farther than the box extents, clamp to the box 
        // then step that distance along the axis to get world coordinate

        var distanceX = Vector2.Dot(directionVector, getXNormal());
        if (distanceX > halfLength.x) 
            distanceX = halfLength.x;
        else if (distanceX < -halfLength.x) 
            distanceX = -halfLength.x;

        var distanceY = Vector2.Dot(directionVector, getYNormal());
        if (distanceY > halfLength.y) 
            distanceY = halfLength.y;
        else if (distanceY < -halfLength.y) 
            distanceY = -halfLength.y;

        return position + distanceX * getXNormal() + distanceY * getYNormal();
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
            return detectCollision(this, other as OBBHull2D);
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
            return detectCollisionResponse(this, other as OBBHull2D);
        }
        throw new System.Exception();
    }
}