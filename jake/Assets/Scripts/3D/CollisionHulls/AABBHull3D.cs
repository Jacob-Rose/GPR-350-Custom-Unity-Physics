using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBHull3D : CollisionHull3D
{
    public Vector3 m_HalfLength;
    public override HullCollision3D DetectCollision(CollisionHull3D other)
    {
        if (other is SphereHull3D)
        {
            return DetectCollision(other as SphereHull3D, this);
        }
        else if (other is AABBHull3D)
        {
            return DetectCollision(this, other as AABBHull3D);
        }
        else if (other is OBBHull3D)
        {
            return DetectCollision(this, other as OBBHull3D);
        }
        else
        {
            throw new System.Exception("Sphere + " + other.GetType() + " are not compatible, add it");
        }
    }

    public Vector3[] GetVertexs()
    {
        Vector3[] verts = new Vector3[8];
        verts[0] = new Vector3(m_HalfLength.x, m_HalfLength.y, m_HalfLength.z); //+++
        verts[1] = new Vector3(-m_HalfLength.x, m_HalfLength.y, m_HalfLength.z); //-++
        verts[2] = new Vector3(m_HalfLength.x, -m_HalfLength.y, m_HalfLength.z); //+-+
        verts[3] = new Vector3(m_HalfLength.x, m_HalfLength.y, -m_HalfLength.z); //++-
        verts[4] = new Vector3(m_HalfLength.x, -m_HalfLength.y, -m_HalfLength.z); //+--
        verts[5] = new Vector3(-m_HalfLength.x, -m_HalfLength.y, m_HalfLength.z); //--+
        verts[6] = new Vector3(-m_HalfLength.x, m_HalfLength.y, -m_HalfLength.z); //-+-
        verts[7] = new Vector3(-m_HalfLength.x, -m_HalfLength.y, -m_HalfLength.z); //---
        return verts;
    }


    public Vector2 getMinMaxProjectionValuesOnNorm(Vector3 norm)
    {
        Vector3[] vertexes = GetVertexs();

        float[] projVals = new float[vertexes.Length];
        int axis = norm.x != 0 ? 0 : norm.y != 0 ? 1 : 2; //just to make a scaler, but in case the x axis is zero, and if they are both zero then what the hell u doing with a normal (0,0)
        for (int i = 0; i < vertexes.Length; i++)
        {
            projVals[i] = Vector3.Project(m_ObjectToWorldTransform.MultiplyPoint(vertexes[i]), norm)[axis] / norm[axis];
        }
        float min = Mathf.Min(projVals);
        float max = Mathf.Max(projVals);
        return new Vector2(min, max);
    }

    public Vector3 GetClosestPoint(Vector3 point)
    {
        float closestPointX = Mathf.Clamp(point.x, m_Position.x - m_HalfLength.x, m_Position.x + m_HalfLength.x);
        float closestPointY = Mathf.Clamp(point.y, m_Position.y - m_HalfLength.y, m_Position.y + m_HalfLength.y);
        float closestPointZ = Mathf.Clamp(point.z, m_Position.z - m_HalfLength.z, m_Position.z + m_HalfLength.z);

        return new Vector3(closestPointX, closestPointY, closestPointZ);
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(m_Position, m_HalfLength * 2);
    }
}
