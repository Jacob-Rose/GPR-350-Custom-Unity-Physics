using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : Particle2D
{
    public Color drawColor = Color.white;
    public override void Start()
    {
        base.Start();
        PhysicsWorld.instance.addObject(this);
        
    }
    public abstract bool detectCollision(CollisionHull2D other);
    public abstract HullCollision2D detectCollisionResponse(CollisionHull2D other);

    public abstract Vector2 GetClosestPoint(Vector2 point);

    public virtual void OnCollision(CollisionHull2D withObject)
    {
        //if any want a trigger on collision
    }

    /*
     * Circle V Circle
     */
    public static bool detectCollision(CircleHull2D a, CircleHull2D b)
    {
        if(a.radius + b.radius >= Vector2.Distance(a.position, b.position))
        {
            return true;
        }
        return false;//failed
    }
    public static HullCollision2D detectCollisionResponse(CircleHull2D a, CircleHull2D b)
    {
        float dist = Vector2.Distance(a.position, b.position);
        if (a.radius + b.radius > dist)
        {
            Vector2 penetration = (a.position - b.position).normalized;
            float penAmount = (a.radius + b.radius) - Vector2.Distance(a.position, b.position);
            HullCollision2D collision = new HullCollision2D(a, b, penetration , penAmount);
            //collision.closingVelocity = -(lft.velocity - rgt.velocity) *(lft.velocity - rgt.velocity).normalized;
            return collision;
        }
        return null;//failed
    }


    /*
     * Circle V AABB
     */
    public static bool detectCollision(CircleHull2D circle, AABBHull2D square)
    {
        Vector2 closestPoint = square.GetClosestPoint(circle.position);
        Debug.DrawLine(new Vector3(closestPoint.x, closestPoint.y, 0), circle.position);
        if(circle.radius > Vector2.Distance(circle.position, closestPoint))
        {
            return true;
        }
        return false;
    }
    /*WORKING*/
    public static HullCollision2D detectCollisionResponse(CircleHull2D circle, AABBHull2D square)
    {
        float dist = Vector2.Distance(circle.position, square.GetClosestPoint(circle.position));
        Vector2 squareClosestPoint = square.GetClosestPoint(circle.position);
        Vector2 circleClosestPoint = circle.GetClosestPoint(squareClosestPoint); //need to do second
        Debug.DrawLine(squareClosestPoint, circleClosestPoint);
        if (circle.radius > dist)
        {
            
            Vector2 pen = squareClosestPoint - circleClosestPoint;
            Vector2 penNorm = pen.normalized;
            if(penNorm.x > penNorm.y)
            {
                pen.y = 0.0f;
            }
            else
            {
                pen.x = 0.0f;
            }
            HullCollision2D collision;
            collision = new HullCollision2D(circle, square, penNorm.normalized, pen.magnitude);
            return collision;
        }
        return null;
    }

    /*
     * Circle V OBB
     */
    public static bool detectCollision(CircleHull2D circle, OBBHull2D square)
    {
        Vector2 closestPoint = square.GetClosestPoint(circle.position);
        Debug.DrawLine(new Vector3(closestPoint.x, closestPoint.y, 0), circle.position);
        if( circle.radius > Vector2.Distance(circle.position, closestPoint))
        {
            return true;
        }
        return false;
    }
    //TODO
    public static HullCollision2D detectCollisionResponse(CircleHull2D circle, OBBHull2D square)
    {
        Vector2 closestPointSquare = square.GetClosestPoint(circle.position);
        Vector2 closestPointCircle = circle.GetClosestPoint(closestPointSquare);
        Debug.DrawLine(new Vector3(closestPointSquare.x, closestPointSquare.y, 0), new Vector3(closestPointCircle.x, closestPointCircle.y, 0));
        if (circle.radius > Vector2.Distance(circle.position, closestPointSquare))
        {
            
            Vector2 penNorm = closestPointSquare- closestPointCircle;
            HullCollision2D collision;
            if (penNorm.x > 0.0f)
            {
                collision = new HullCollision2D(circle, square, penNorm.normalized, penNorm.magnitude);
            }
            else
            {
                collision = new HullCollision2D(circle, square, penNorm.normalized, penNorm.magnitude);
            }
            return collision;
        }
        return null;
    }

    /*
     * AABB v AABB
     */
    public static bool detectCollision(AABBHull2D a, AABBHull2D b)
    {
        for(int i = 0; i < 2; i++) //do for each axis
        {
            Vector2 minMaxA = new Vector2(a.position[i] - a.halfLength[i], a.position[i] + a.halfLength[i]);
            Vector2 minMaxB = new Vector2(b.position[i] - b.halfLength[i], b.position[i] + b.halfLength[i]);
            if (!detectCollisionFromMinMax(minMaxA,minMaxB))
            {
                return false;
            }
        }
        return true;
    }

    public static HullCollision2D detectCollisionResponse(AABBHull2D a, AABBHull2D b)
    {
        Dictionary<Vector2, float> axisValues = new Dictionary<Vector2, float>(); //norm to the overlap value
        axisValues.Add(Vector2.up, checkAxisPenetration(a, b, Vector2.up));
        axisValues.Add(Vector2.right, checkAxisPenetration(a, b, Vector2.right));
        Dictionary<Vector2, float>.Enumerator enumerator = axisValues.GetEnumerator();
        enumerator.MoveNext();
        KeyValuePair<Vector2, float> bestPenetration = enumerator.Current; //need to set one to compare too, no null value to just allow all checking in the for loop
        if (bestPenetration.Value == 0.0f)
        {
            return null;
        }
        while (enumerator.MoveNext())
        {
            if (Mathf.Abs(enumerator.Current.Value) < Mathf.Abs(bestPenetration.Value))
            {
                bestPenetration = enumerator.Current;
            }
            if (bestPenetration.Value == 0.0f)
            {
                return null;
            }
        }
        HullCollision2D collision;
        collision = new HullCollision2D(a, b, bestPenetration.Key, Mathf.Abs(bestPenetration.Value));
        return collision;
    }

    /*
    * VERIFIED WORKING, based off OBB v OBB Collision
    */
    public static bool detectCollision(OBBHull2D a, AABBHull2D b)
    {
        if( checkAxis(a, b, a.getXNormal()) && 
            checkAxis(a, b, a.getYNormal()) && 
            checkAxis(a, b, Vector2.left) && 
            checkAxis(a, b, Vector2.up))
        {
            return true;
        }
        return false;
    }

    public static HullCollision2D detectCollisionResponse(OBBHull2D a, AABBHull2D b)
    {
        Dictionary<Vector2, float> axisValues = new Dictionary<Vector2, float>(); //norm to the overlap value
        axisValues.Add(Vector2.up, checkAxisPenetration(a, b, Vector2.up));
        axisValues.Add(Vector2.right, checkAxisPenetration(a, b, Vector2.right));
        if (a.rotation != 0.0f)
        {
            axisValues.Add(a.getXNormal(), checkAxisPenetration(a, b, a.getXNormal()));
            axisValues.Add(a.getYNormal(), checkAxisPenetration(a, b, a.getYNormal()));
        }
        Dictionary<Vector2, float>.Enumerator enumerator = axisValues.GetEnumerator();
        enumerator.MoveNext();
        KeyValuePair<Vector2, float> bestPenetration = enumerator.Current; //need to set one to compare too, no null value to just allow all checking in the for loop
        if (bestPenetration.Value == 0.0f)
        {
            return null;
        }
        while (enumerator.MoveNext())
        {
            if (Mathf.Abs(enumerator.Current.Value) < Mathf.Abs(bestPenetration.Value))
            {
                bestPenetration = enumerator.Current;
            }
            if (bestPenetration.Value == 0.0f)
            {
                return null;
            }
        }
        HullCollision2D collision;
        Vector2 norm = bestPenetration.Key;
        
        
        if(Vector2.Dot(a.velocity, norm) >= 0.0f)
        {
            norm *= -1.0f;
        }
        
        
        collision = new HullCollision2D(a, b, norm, Mathf.Abs(bestPenetration.Value));
        return collision;
    }


    /*
     * Verified Working, possible error in using 4 checkaxis, i think i missed a check somewhere deeper in the code
     */
    public static bool detectCollision(OBBHull2D lft, OBBHull2D rgt)
    {
        //check each axis on each side, need to make better
        if( checkAxis(lft, rgt, lft.getXNormal()) && 
            checkAxis(lft, rgt, lft.getYNormal()) && 
            checkAxis(lft, rgt, rgt.getXNormal()) && 
            checkAxis(lft, rgt, rgt.getYNormal()))
        {
            return true;
        }
        return false;
    }

    public static HullCollision2D detectCollisionResponse(OBBHull2D a, OBBHull2D b)
    {
        Dictionary<Vector2, float> axisValues = new Dictionary<Vector2, float>(); //norm to the overlap value
        axisValues.Add(a.getXNormal(), checkAxisPenetration(a, b, a.getXNormal()));
        axisValues.Add(a.getYNormal(), checkAxisPenetration(a, b, a.getYNormal()));
        if (a.rotation != b.rotation)
        {
            axisValues.Add(b.getXNormal(), checkAxisPenetration(a, b, b.getXNormal()));
            axisValues.Add(b.getYNormal(), checkAxisPenetration(a, b, b.getYNormal()));
        }
        Dictionary<Vector2, float>.Enumerator enumerator = axisValues.GetEnumerator();
        enumerator.MoveNext();
        Vector2 pos = a.position;
        KeyValuePair<Vector2, float> bestPenetration = enumerator.Current; //need to set one to compare too, no null value to just allow all checking in the for loop
        if (bestPenetration.Value == 0.0f)
        {
            return null;
        }
        while (enumerator.MoveNext())
        {
            if (Mathf.Abs(enumerator.Current.Value) < Mathf.Abs(bestPenetration.Value))
            {
                bestPenetration = enumerator.Current;
            }
            if (bestPenetration.Value == 0.0f)
            {
                return null;
            }
        }
        HullCollision2D collision;
        Vector2 norm = bestPenetration.Key;
        if (bestPenetration.Value <= 0.0f) //ooh this was an easy thing to miss
        {
            norm *= -1.0f;
        }

        collision = new HullCollision2D(a, b, norm, Mathf.Abs(bestPenetration.Value));
        //collision.contactPoint = pos;
        return collision;
    }

    public static bool checkAxis(OBBHull2D a, OBBHull2D b, Vector2 norm)
    {
        Vector2 aProjValues = getMinMaxProjectionValuesOnNorm(a, norm);
        Vector2 bProjValues = getMinMaxProjectionValuesOnNorm(b, norm);
        Debug.DrawLine(aProjValues.x * norm, aProjValues.y * norm, Color.blue); //this is dumb useful, the projected line
        Debug.DrawLine(bProjValues.x * norm, bProjValues.y * norm, Color.red);//this is dumb useful, the projected line

        return detectCollisionFromMinMax(aProjValues, bProjValues);
    }

    public static float checkAxisPenetration(OBBHull2D a, OBBHull2D b, Vector2 norm)
    {
        Vector2 aProjValues = getMinMaxProjectionValuesOnNorm(a, norm);
        Vector2 bProjValues = getMinMaxProjectionValuesOnNorm(b, norm);
        Debug.DrawLine(aProjValues.x * norm, aProjValues.y * norm, Color.blue); //this is dumb useful, the projected line
        Debug.DrawLine(bProjValues.x * norm, bProjValues.y * norm, Color.red);//this is dumb useful, the projected line

        if(! detectCollisionFromMinMax(aProjValues, bProjValues))
        {
            return 0.0f;
        }
        else
        {
            return calculateMinMaxCollisionOverlap(aProjValues, bProjValues);
        }
    }

    public static float checkAxisPenetration(OBBHull2D a, AABBHull2D b, Vector2 norm)
    {
        Vector2 aProjValues = getMinMaxProjectionValuesOnNorm(a, norm);
        Vector2 bProjValues = getMinMaxProjectionValuesOnNorm(b, norm);
        Debug.DrawLine(aProjValues.x * norm, aProjValues.y * norm, Color.blue); //this is dumb useful, the projected line
        Debug.DrawLine(bProjValues.x * norm, bProjValues.y * norm, Color.red);//this is dumb useful, the projected line

        if (!detectCollisionFromMinMax(aProjValues, bProjValues))
        {
            return 0.0f;
        }
        else
        {
            return calculateMinMaxCollisionOverlap(aProjValues, bProjValues);
        }
    }

    public static float checkAxisPenetration(AABBHull2D a, AABBHull2D b, Vector2 norm)
    {
        Vector2 aProjValues = getMinMaxProjectionValuesOnNorm(a, norm);
        Vector2 bProjValues = getMinMaxProjectionValuesOnNorm(b, norm);
        Debug.DrawLine(aProjValues.x * norm, aProjValues.y * norm, Color.blue); //this is dumb useful, the projected line
        Debug.DrawLine(bProjValues.x * norm, bProjValues.y * norm, Color.red);//this is dumb useful, the projected line

        if (!detectCollisionFromMinMax(aProjValues, bProjValues))
        {
            return 0.0f;
        }
        else
        {
            return calculateMinMaxCollisionOverlap(aProjValues, bProjValues);
        }
    }

    public static bool checkAxis(OBBHull2D a, AABBHull2D b, Vector2 norm)
    {
        Vector2 aProjValues = getMinMaxProjectionValuesOnNorm(a, norm);
        Vector2 bProjValues = getMinMaxProjectionValuesOnNorm(b, norm);
        Debug.DrawLine(aProjValues.x * norm, aProjValues.y * norm, Color.blue); //this is dumb useful, the projected line
        Debug.DrawLine(bProjValues.x * norm, bProjValues.y * norm, Color.red);//this is dumb useful, the projected line

        return detectCollisionFromMinMax(aProjValues, bProjValues);
    }
    public static Vector2 getMinMaxProjectionValuesOnNorm(OBBHull2D box, Vector2 norm)
    {
        float a, b, c, d;
        int axis = norm.x != 0 ? 0 : 1; //just to make a scaler, but in case the x axis is zero, and if they are both zero then what the hell u doing with a normal (0,0)
        a = proj(box.getTopLeftPos(), norm)[axis] / norm[axis];
        b = proj(box.getTopRightPos(), norm)[axis] / norm[axis];
        c = proj(box.getBottomLeftPos(), norm)[axis] / norm[axis];
        d = proj(box.getBottomRightPos(), norm)[axis] / norm[axis];
        float min = Mathf.Min(new float[] { a, b, c, d });
        float max = Mathf.Max(new float[] { a, b, c, d });
        return new Vector2(min, max);
    }

    public static Vector4 getMinMaxProjectionValuesOnNorm(AABBHull2D hull, Vector2 norm)
    {
        float a, b, c, d;
        int axis = norm.x != 0 ? 0 : 1; //just to make a scaler, but in case the x axis is zero, and if they are both zero then what the hell u doing with a normal (0,0)
        a = proj(hull.position + hull.halfLength, norm)[axis] / norm[axis];
        b = proj(hull.position - hull.halfLength, norm)[axis] / norm[axis];
        c = proj(hull.position + new Vector2(hull.halfLength.x, -hull.halfLength.y), norm)[axis] / norm[axis];
        d = proj(hull.position + new Vector2(-hull.halfLength.x, +hull.halfLength.y), norm)[axis] / norm[axis];
        float min = Mathf.Min(new float[] { a, b, c, d });
        float max = Mathf.Max(new float[] { a, b, c, d });
        return new Vector2(min, max);
    }

    public static Vector2 getMinMaxProjectionValuesOnNorm(CircleHull2D circle, Vector2 norm)
    {
        float a, b;
        int axis = norm.x != 0 ? 0 : 1; //just to make a scaler, but in case the x axis is zero, and if they are both zero then what the hell u doing with a normal (0,0)
        a = (proj(circle.position, norm)[axis] - circle.radius)/ norm[axis];
        b = (proj(circle.position, norm)[axis] + circle.radius)/ norm[axis];
        return new Vector2(a, b);
    }
    public static bool detectCollisionFromMinMax(Vector2 aMinMax, Vector2 bMinMax)
    {
        if (aMinMax.x > bMinMax.x) //amin > bmin, thus a is left of b
        {
            if (aMinMax.x < bMinMax.y) //amin < bmax, thus within bounds
                return true;
            else
                return false;
        }
        else
        {
            if (bMinMax.x < aMinMax.y)
                return true;
            else
                return false;
        }
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

    public static Vector2 proj(Vector2 point, Vector2 normal)
    {
        return Vector2.Dot(point, normal) * normal;
    }
}