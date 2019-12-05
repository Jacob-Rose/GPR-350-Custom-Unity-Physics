using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBHull3D : CollisionHull3D
{
    public Vector3 m_HalfLength;

    public Vector3 getXNormal()
    {
        return new Vector3(m_WorldToObjectTransform.m00, m_WorldToObjectTransform.m01, m_WorldToObjectTransform.m02).normalized;
    }
    public Vector3 getYNormal()
    {
        return new Vector3(m_WorldToObjectTransform.m10, m_WorldToObjectTransform.m11, m_WorldToObjectTransform.m12).normalized;
    }
    public Vector3 getZNormal()
    {
        return new Vector3(m_WorldToObjectTransform.m20, m_WorldToObjectTransform.m21, m_WorldToObjectTransform.m22).normalized;
    }

    public Vector3[] GetVertexs()
    {
        Vector3[] verts = new Vector3[8];
        verts[0] = m_ObjectToWorldTransform.MultiplyPoint(new Vector3(m_HalfLength.x, m_HalfLength.y, m_HalfLength.z)); //+++
        verts[1] = m_ObjectToWorldTransform.MultiplyPoint(new Vector3(-m_HalfLength.x, m_HalfLength.y, m_HalfLength.z)); //-++
        verts[2] = m_ObjectToWorldTransform.MultiplyPoint(new Vector3(m_HalfLength.x, -m_HalfLength.y, m_HalfLength.z)); //+-+
        verts[3] = m_ObjectToWorldTransform.MultiplyPoint(new Vector3(m_HalfLength.x, m_HalfLength.y, -m_HalfLength.z)); //++-
        verts[4] = m_ObjectToWorldTransform.MultiplyPoint(new Vector3(m_HalfLength.x, -m_HalfLength.y, -m_HalfLength.z)); //+--
        verts[5] = m_ObjectToWorldTransform.MultiplyPoint(new Vector3(-m_HalfLength.x, -m_HalfLength.y, m_HalfLength.z)); //--+
        verts[6] = m_ObjectToWorldTransform.MultiplyPoint(new Vector3(-m_HalfLength.x, m_HalfLength.y, -m_HalfLength.z)); //-+-
        verts[7] = m_ObjectToWorldTransform.MultiplyPoint(new Vector3(-m_HalfLength.x, -m_HalfLength.y, -m_HalfLength.z)); //---
        return verts;
    }

    public override Vector2 getMinMaxProjectionValuesOnNorm(Vector3 norm)
    {
        Vector3[] vertexes = GetVertexs();
        float[] projVals = new float[vertexes.Length];

        int axis = norm.x != 0 ? 0 : norm.y != 0 ? 1 : 2; //just to make a scaler, but in case the x axis is zero, and if they are both zero then what the hell u doing with a normal (0,0)
        for(int i = 0; i < vertexes.Length; i++)
        {
            projVals[i] = Vector3.Project(vertexes[i], norm)[axis] / norm[axis];
        }
        float min = Mathf.Min(projVals);
        float max = Mathf.Max(projVals);
        return new Vector2(min, max);
    }

    public Vector3 GetClosestPoint(Vector3 point)
    {
        Vector3 directionVector = point - m_Position;

        var distanceX = Vector3.Dot(directionVector, getXNormal());
        if (distanceX > m_HalfLength.x)
            distanceX = m_HalfLength.x;
        else if (distanceX < -m_HalfLength.x)
            distanceX = -m_HalfLength.x;

        var distanceY = Vector3.Dot(directionVector, getYNormal());
        if (distanceY > m_HalfLength.y)
            distanceY = m_HalfLength.y;
        else if (distanceY < -m_HalfLength.y)
            distanceY = -m_HalfLength.y;

        var distanceZ = Vector3.Dot(directionVector, getZNormal());
        if (distanceZ > m_HalfLength.z)
            distanceZ = m_HalfLength.z;
        else if (distanceZ < -m_HalfLength.z)
            distanceZ = -m_HalfLength.z;

        return m_Position + (distanceX * getXNormal()) + (distanceY * getYNormal()) + (distanceZ * getZNormal());
    }

    public override HullCollision3D DetectCollision(CollisionHull3D other)
    {
        if (other is SphereHull3D)
        {
            return DetectCollision(other as SphereHull3D, this);
        }
        else if (other is AABBHull3D)
        {
            return DetectCollision(other as AABBHull3D, this);
        }
        else if (other is OBBHull3D)
        {
            return DetectCollision(this, other as OBBHull3D);
        }
        else
        {
            throw new System.Exception(GetType().ToString() + other.GetType().ToString() + " are not compatible, add it");
        }
    }

    public void OnDrawGizmos()
    {
        Vector3 offsetPos = Vector3.zero;
        Quaternion offsetRot = Quaternion.identity;
        if(!Application.isPlaying)
        {
            offsetPos = transform.position;
            offsetRot = transform.rotation;
        }
        Matrix4x4 oldMat = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(m_Position + offsetPos, m_Rotation * offsetRot , Vector3.one);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, m_HalfLength * 2.0f);
        Gizmos.matrix = oldMat;
    }
}
