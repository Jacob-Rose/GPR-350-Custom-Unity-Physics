using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CircleHull2D : CollisionHull2D
{
    public float radius;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 lastPos = new Vector2(radius, 0) + position;
        //draw each segment, added 0.01f to make sure any float errors dont affect it and the final side is drawn
        for (float theta = Mathf.PI * 0.1f; theta <= (Mathf.PI * 2) + 0.01f; theta += Mathf.PI * 0.1f)
        {
            Vector2 posA = new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta)) + position;
            Gizmos.DrawLine(lastPos, posA);
            lastPos = posA;
        }
    }


    public override HullCollision2D detectCollision(CollisionHull2D other)
    {
        if (other is CircleHull2D)
        {
            return detectCollision(this, other as CircleHull2D);
        }
        else if (other is AABBHull2D)
        {
            return detectCollision(this, other as AABBHull2D);
        }
        else if(other is OBBHull2D)
        {
            return detectCollision(this, other as OBBHull2D);
        }
        throw new System.Exception(); //oops, looks like you added another collision type, thats a no no
    }
}
