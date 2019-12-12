using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull3D : Particle3D
{
    public float m_Restitution = 0.5f;
    public abstract HullCollision3D DetectCollision(CollisionHull3D other);
    public abstract void OnCollision(HullCollision3D coll);
    public abstract Vector2 getMinMaxProjectionValuesOnNorm(Vector3 norm);

    public bool m_IsTrigger = false;

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
            Vector3[] contactPoints = new Vector3[] { Vector3.zero };
            return new HullCollision3D(a, b, penetration.normalized, penetration.magnitude, contactPoints);
        }
        return null;
    }

    public HullCollision3D DetectCollision(SphereHull3D a, AABBHull3D b)
    {
        Dictionary<Vector3, float> axisValues = new Dictionary<Vector3, float>(); //norm to the overlap value
        axisValues.Add(Vector3.right, checkAxisPenetration(a, b, Vector3.right));
        axisValues.Add(Vector3.up, checkAxisPenetration(a, b, Vector3.up));
        axisValues.Add(Vector3.forward, checkAxisPenetration(a, b, Vector3.forward));
        var enumerator = axisValues.GetEnumerator();
        Vector3[] contactPoints = new Vector3[1];
        contactPoints[0] = Vector3.zero;
        KeyValuePair<Vector3, float>? bestPenetration = null; //need to set one to compare too, no null value to just allow all checking in the for loop
        while (enumerator.MoveNext())
        {
            if (!bestPenetration.HasValue
                || Mathf.Abs(enumerator.Current.Value) < Mathf.Abs(bestPenetration.Value.Value))
            {
                bestPenetration = enumerator.Current;
            }
            contactPoints[0] += enumerator.Current.Key * enumerator.Current.Value;
            if (enumerator.Current.Value >= 0.0f)
            {
                return null;
            }
        }
        return new HullCollision3D(a, b, bestPenetration.Value.Key, bestPenetration.Value.Value, contactPoints);
    }

    public HullCollision3D DetectCollision(SphereHull3D a, OBBHull3D b)
    {
        Dictionary<Vector3, float> axisValues = new Dictionary<Vector3, float>(); //norm to the overlap value
        HashSet<Vector3> axisToCheck = new HashSet<Vector3>();
        axisToCheck.Add(b.getXNormal());
        axisToCheck.Add(b.getYNormal());
        axisToCheck.Add(b.getZNormal());
        foreach (Vector3 norm in axisToCheck)
        {
            if (norm != Vector3.zero && (!axisValues.ContainsKey(norm) || !axisValues.ContainsKey(-norm)))
            {
                float axisPenetration = checkAxisPenetration(a, b, norm);
                if (axisPenetration < 0)
                {
                    axisValues.Add(-norm, -axisPenetration);
                }
                else
                {
                    axisValues.Add(norm, axisPenetration);
                }

            }
        }

        var enumerator = axisValues.GetEnumerator();
        Vector3[] contactPoints = new Vector3[1];
        KeyValuePair<Vector3, float>? bestPenetration = null; //need to set one to compare too, no null value to just allow all checking in the for loop
        while (enumerator.MoveNext())
        {
            if (!bestPenetration.HasValue || enumerator.Current.Value < bestPenetration.Value.Value)
            {
                bestPenetration = enumerator.Current;
            }
            if (enumerator.Current.Value == 0)
            {
                return null;
            }
        }
        // Which face is in contact?
        contactPoints[0] = Vector3.zero;
        return new HullCollision3D(a, b, bestPenetration.Value.Key, bestPenetration.Value.Value, contactPoints);
    }

    public HullCollision3D DetectCollision(AABBHull3D a, AABBHull3D b)
    {
        Dictionary<Vector3, float> axisValues = new Dictionary<Vector3, float>(); //norm to the overlap value
        HashSet<Vector3> axisToCheck = new HashSet<Vector3>();
        axisToCheck.Add(a.getXNormal());
        axisToCheck.Add(a.getYNormal());
        axisToCheck.Add(a.getZNormal());
        foreach (Vector3 norm in axisToCheck)
        {
            if (norm != Vector3.zero && (!axisValues.ContainsKey(norm) || !axisValues.ContainsKey(-norm)))
            {
                float axisPenetration = checkAxisPenetration(a, b, norm);
                if (axisPenetration < 0)
                {
                    axisValues.Add(-norm, -axisPenetration);
                }
                else
                {
                    axisValues.Add(norm, axisPenetration);
                }

            }
        }

        var enumerator = axisValues.GetEnumerator();
        Vector3[] contactPoints = new Vector3[1];
        KeyValuePair<Vector3, float>? bestPenetration = null; //need to set one to compare too, no null value to just allow all checking in the for loop
        while (enumerator.MoveNext())
        {
            if (!bestPenetration.HasValue || enumerator.Current.Value < bestPenetration.Value.Value)
            {
                bestPenetration = enumerator.Current;
            }
            if (enumerator.Current.Value == 0)
            {
                return null;
            }
        }
        // Which face is in contact?
        contactPoints[0] = Vector3.zero;
        return new HullCollision3D(a, b, bestPenetration.Value.Key, bestPenetration.Value.Value, contactPoints);
    }

    public HullCollision3D DetectCollision(AABBHull3D a, OBBHull3D b)
    {
        Dictionary<Vector3, float> axisValues = new Dictionary<Vector3, float>(); //norm to the overlap value
        HashSet<Vector3> axisToCheck = new HashSet<Vector3>();
        axisToCheck.Add(a.getXNormal());
        axisToCheck.Add(a.getYNormal());
        axisToCheck.Add(a.getZNormal());
        axisToCheck.Add(b.getXNormal());
        axisToCheck.Add(b.getYNormal());
        axisToCheck.Add(b.getZNormal());
        axisToCheck.Add(Vector3.Cross(a.getXNormal(), b.getXNormal())); //xx
        axisToCheck.Add(Vector3.Cross(a.getXNormal(), b.getYNormal())); //xy
        axisToCheck.Add(Vector3.Cross(a.getXNormal(), b.getZNormal())); //xz
        axisToCheck.Add(Vector3.Cross(a.getYNormal(), b.getXNormal())); //yx
        axisToCheck.Add(Vector3.Cross(a.getYNormal(), b.getYNormal())); //yy
        axisToCheck.Add(Vector3.Cross(a.getYNormal(), b.getZNormal())); //yz
        axisToCheck.Add(Vector3.Cross(a.getZNormal(), b.getXNormal())); //zx
        axisToCheck.Add(Vector3.Cross(a.getZNormal(), b.getYNormal())); //zy
        axisToCheck.Add(Vector3.Cross(a.getZNormal(), b.getZNormal())); //zz
        foreach (Vector3 norm in axisToCheck)
        {
            if (norm != Vector3.zero && (!axisValues.ContainsKey(norm) || !axisValues.ContainsKey(-norm)))
            {
                float axisPenetration = checkAxisPenetration(a, b, norm);
                if (axisPenetration < 0)
                {
                    axisValues.Add(-norm, -axisPenetration);
                }
                else
                {
                    axisValues.Add(norm, axisPenetration);
                }

            }
        }

        var enumerator = axisValues.GetEnumerator();
        Vector3[] contactPoints = new Vector3[1];
        KeyValuePair<Vector3, float>? bestPenetration = null; //need to set one to compare too, no null value to just allow all checking in the for loop
        while (enumerator.MoveNext())
        {
            if (!bestPenetration.HasValue || enumerator.Current.Value < bestPenetration.Value.Value)
            {
                bestPenetration = enumerator.Current;
            }
            if (enumerator.Current.Value == 0)
            {
                return null;
            }
        }
        // Which face is in contact?
        contactPoints[0] = Vector3.zero;
        return new HullCollision3D(a, b, bestPenetration.Value.Key, bestPenetration.Value.Value, contactPoints);
    }

    public HullCollision3D DetectCollision(OBBHull3D a, OBBHull3D b)
    {
        Dictionary<Vector3, float> axisValues = new Dictionary<Vector3, float>(); //norm to the overlap value
        HashSet<Vector3> axisToCheck = new HashSet<Vector3>();
        axisToCheck.Add(a.getXNormal()); 
        axisToCheck.Add(a.getYNormal()); 
        axisToCheck.Add(a.getZNormal());
        axisToCheck.Add(b.getXNormal());
        axisToCheck.Add(b.getYNormal());
        axisToCheck.Add(b.getZNormal());
        axisToCheck.Add(Vector3.Cross(a.getXNormal(), b.getXNormal())); //xx
        axisToCheck.Add(Vector3.Cross(a.getXNormal(), b.getYNormal())); //xy
        axisToCheck.Add(Vector3.Cross(a.getXNormal(), b.getZNormal())); //xz
        axisToCheck.Add(Vector3.Cross(a.getYNormal(), b.getXNormal())); //yx
        axisToCheck.Add(Vector3.Cross(a.getYNormal(), b.getYNormal())); //yy
        axisToCheck.Add(Vector3.Cross(a.getYNormal(), b.getZNormal())); //yz
        axisToCheck.Add(Vector3.Cross(a.getZNormal(), b.getXNormal())); //zx
        axisToCheck.Add(Vector3.Cross(a.getZNormal(), b.getYNormal())); //zy
        axisToCheck.Add(Vector3.Cross(a.getZNormal(), b.getZNormal())); //zz
        foreach(Vector3 norm in axisToCheck)
        {
            if (norm != Vector3.zero && (!axisValues.ContainsKey(norm) || !axisValues.ContainsKey(-norm)))
            {
                float axisPenetration = checkAxisPenetration(a, b, norm);
                if (axisPenetration < 0)
                {
                    axisValues.Add(-norm, -axisPenetration);
                }
                else
                {
                    axisValues.Add(norm, axisPenetration);
                }
                
            }
        }

        var enumerator = axisValues.GetEnumerator();
        Vector3[] contactPoints = new Vector3[1];
        KeyValuePair<Vector3, float>? bestPenetration = null; //need to set one to compare too, no null value to just allow all checking in the for loop
        while (enumerator.MoveNext())
        {
            if (!bestPenetration.HasValue || enumerator.Current.Value < bestPenetration.Value.Value)
            {
                bestPenetration = enumerator.Current;
            }
            if (enumerator.Current.Value == 0)
            {
                return null;
            }
        }
        // Which face is in contact?
        contactPoints[0] = Vector3.zero;
        return new HullCollision3D(a, b, bestPenetration.Value.Key, bestPenetration.Value.Value, contactPoints);

    }

    public static float checkAxisPenetration(CollisionHull3D a, CollisionHull3D b, Vector3 norm)
    {
        Vector2 aProjValues = a.getMinMaxProjectionValuesOnNorm(norm);
        //Debug.DrawLine(aProjValues.x * norm, aProjValues.y * norm, Color.red);
        Vector2 bProjValues = b.getMinMaxProjectionValuesOnNorm(norm);
        //Debug.DrawLine(bProjValues.x * norm, bProjValues.y * norm, Color.blue);
        return calculateMinMaxCollisionOverlap(aProjValues, bProjValues);
    }

    public static float calculateMinMaxCollisionOverlap(Vector2 aMinMax, Vector2 bMinMax)
    {
        if (aMinMax.x >= bMinMax.x && aMinMax.x <= bMinMax.y)
        {
            return aMinMax.x - bMinMax.y;
        }
        else if(bMinMax.x >= aMinMax.x && bMinMax.x <= aMinMax.y)
        {
            return bMinMax.x - aMinMax.y;
        }
        else
        {
            return 0.0f;
        }
    }
}
