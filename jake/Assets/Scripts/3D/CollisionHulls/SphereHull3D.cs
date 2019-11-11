using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHull3D : CollisionHull3D
{
    public float m_Radius = 2.0f;

    public override HullCollision3D DetectCollision(CollisionHull3D other)
    {
        if(other is SphereHull3D)
        {
            return DetectCollision(this, other as SphereHull3D);
        }
        else if(other is AABBHull3D)
        {
            return DetectCollision(this, other as AABBHull3D);
        }
        else if(other is OBBHull3D)
        {
            return DetectCollision(this, other as OBBHull3D);
        }
        else
        {
            throw new System.Exception("Sphere + " + other.GetType() + " are not compatible, add it");
        }
    }

    public Vector3 GetClosestPoint(Vector3 point)
    {
        Vector3 dir = point - m_Position;
        dir.Normalize();
        return m_Position + (dir * m_Radius);
    }

    public override Vector2 getMinMaxProjectionValuesOnNorm(Vector3 norm)
    {
        int axis = norm.x != 0 ? 0 : norm.y != 0 ? 1 : 2;
        float centerProj = Vector3.Project(m_Position, norm)[axis] / norm[axis];
        return new Vector2(centerProj - m_Radius, centerProj + m_Radius); //pretty easy
    }

    private void OnDrawGizmos()
    {
        Vector3 offset = Vector3.zero;
        if(!Application.isPlaying)
        {
            offset = transform.position;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_Position + offset, m_Radius);
    }
}
