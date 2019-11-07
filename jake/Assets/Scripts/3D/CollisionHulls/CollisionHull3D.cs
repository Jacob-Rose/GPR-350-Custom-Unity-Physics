using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull3D : Particle3D
{
    public float m_Restitution = 0.5f;
    public abstract HullCollision3D DetectCollision(CollisionHull3D other);

    public override void Start()
    {
        base.Start();
        PhysicsWorld3D.instance.addObject(this);
    }

    public HullCollision3D DetectCollision(SphereHull3D a, SphereHull3D b)
    {
        float dist = Vector3.Distance(a.m_Position, b.m_Position);
        if (a.m_Radius + b.m_Radius >= dist)
        {
            Vector3 penetration = (a.m_Position - b.m_Position).normalized;
            Vector3 contactPoint = penetration / 2;
            return new HullCollision3D(a, b, penetration.normalized, penetration.magnitude, new Vector3[] { contactPoint });
        }
        return null;
    }

    public HullCollision3D DetectCollision(SphereHull3D a, AABBHull3D b)
    {
        Vector3 squareClosestPoint = b.GetClosestPoint(a.m_Position);
        Vector3 circleClosestPoint = a.GetClosestPoint(squareClosestPoint); //do this second
        float dist = Vector3.Distance(a.m_Position, b.GetClosestPoint(a.m_Position));

        if (a.m_Radius > dist)
        {
            Vector3 pen = squareClosestPoint - circleClosestPoint;
            Vector3 penNorm = pen.normalized;
            float maxAxis = Mathf.Max(new float[] { penNorm[0], penNorm[1], penNorm[2] });
            Vector3[] contactPoints = new Vector3[1];
            contactPoints[0] = pen * 0.5f;
            return new HullCollision3D(a, b, penNorm.normalized, pen.magnitude, contactPoints);
        }
        return null;
    }

    public HullCollision3D DetectCollision(SphereHull3D a, OBBHull3D b)
    {
        Vector3 closestPointSquare = b.GetClosestPoint(a.m_Position);
        Vector3 closestPointCircle = a.GetClosestPoint(closestPointSquare);
        if (a.m_Radius > Vector2.Distance(a.m_Position, closestPointSquare))
        {
            Vector3 penetration = closestPointSquare - closestPointCircle;
            Vector3[] contactPoints = new Vector3[1];
            contactPoints[0] = penetration * 0.5f;
            return new HullCollision3D(a, b, penetration.normalized, penetration.magnitude, contactPoints);
        }
        return null;
    }

    //WORKING
    public HullCollision3D DetectCollision(AABBHull3D a, AABBHull3D b)
    {
        Dictionary<Vector3, float> axisValues = new Dictionary<Vector3, float>(); //norm to the overlap value
        float min, max;
        max = (a.m_HalfLength.x + a.m_Position.x);
        min = (-b.m_HalfLength.x + b.m_Position.x);
        axisValues.Add(Vector3.right, min - max);
        max = (a.m_HalfLength.y + a.m_Position.y);
        min = (-b.m_HalfLength.y + b.m_Position.y);
        axisValues.Add(Vector3.up, min - max);
        max = (a.m_HalfLength.z + a.m_Position.z);
        min = (-b.m_HalfLength.z + b.m_Position.z);
        axisValues.Add(Vector3.forward, min - max);
        var enumerator = axisValues.GetEnumerator();
        KeyValuePair<Vector3, float>? bestPenetration = null; //need to set one to compare too, no null value to just allow all checking in the for loop
        while (enumerator.MoveNext())
        {
            if (!bestPenetration.HasValue
                || Mathf.Abs(enumerator.Current.Value) < Mathf.Abs(bestPenetration.Value.Value))
            {
                bestPenetration = enumerator.Current;
            }
            if (enumerator.Current.Value >= 0.0f)
            {
                return null;
            }
        }
        Vector3[] contactPoints = new Vector3[1];
        contactPoints[0] = bestPenetration.Value.Key;
        return new HullCollision3D(a, b, bestPenetration.Value.Key, bestPenetration.Value.Value, contactPoints);
    }

    public HullCollision3D DetectCollision(AABBHull3D a, OBBHull3D b)
    {
        return null;
    }
    public HullCollision3D DetectCollision(OBBHull3D a, OBBHull3D b)
    {
        Dictionary<Vector3, float> axisValues = new Dictionary<Vector3, float>(); //norm to the overlap value
        axisValues.Add(a.getXNormal(), checkAxisPenetration(a, b, a.getXNormal()));
        axisValues.Add(a.getYNormal(), checkAxisPenetration(a, b, a.getYNormal()));
        axisValues.Add(a.getZNormal(), checkAxisPenetration(a, b, a.getZNormal()));
        if (!axisValues.ContainsKey(b.getXNormal()))
        {
            axisValues.Add(b.getXNormal(), checkAxisPenetration(a, b, b.getXNormal()));
        }
        if (!axisValues.ContainsKey(b.getYNormal()))
        {
            axisValues.Add(b.getYNormal(), checkAxisPenetration(a, b, b.getYNormal()));
        }
        if (!axisValues.ContainsKey(b.getZNormal()))
        {
            axisValues.Add(b.getZNormal(), checkAxisPenetration(a, b, b.getZNormal()));
        }
        var enumerator = axisValues.GetEnumerator();
        KeyValuePair<Vector3, float>? bestPenetration = null; //need to set one to compare too, no null value to just allow all checking in the for loop
        while (enumerator.MoveNext())
        {
            if (!bestPenetration.HasValue || Mathf.Abs(enumerator.Current.Value) < Mathf.Abs(bestPenetration.Value.Value))
            {
                bestPenetration = enumerator.Current;
            }
            if (bestPenetration.Value.Value == 0.0f)
            {
                return null;
            }
        }
        Vector3 norm = bestPenetration.Value.Key;
        Vector3[] contactPoints = new Vector3[1];
        contactPoints[0] = Vector3.zero; //todo
        return new HullCollision3D(a, b, norm, bestPenetration.Value.Value, contactPoints);

    }

    public static float checkAxisPenetration(OBBHull3D a, OBBHull3D b, Vector3 norm)
    {
        Vector2 aProjValues = a.getMinMaxProjectionValuesOnNorm(norm);
        Vector2 bProjValues = b.getMinMaxProjectionValuesOnNorm(norm);
        //Debug.DrawLine(aProjValues.x * norm, aProjValues.y * norm, Color.blue); //this is dumb useful, the projected line
        //Debug.DrawLine(bProjValues.x * norm, bProjValues.y * norm, Color.red);//this is dumb useful, the projected line
        return calculateMinMaxCollisionOverlap(aProjValues, bProjValues);
    }

    public static float calculateMinMaxCollisionOverlap(Vector2 aMinMax, Vector2 bMinMax)
    {
        if (aMinMax.x > bMinMax.x) //amin > bmin, thus a is left of b
        {
            if (aMinMax.x < bMinMax.y) //amin < bmax, thus within bounds
                return aMinMax.x - bMinMax.y;
            else
                return 0.0f;
        }
        else
        {
            if (bMinMax.x < aMinMax.y)
                return bMinMax.x - aMinMax.y;
            else
                return 0.0f;
        }
    }
}
