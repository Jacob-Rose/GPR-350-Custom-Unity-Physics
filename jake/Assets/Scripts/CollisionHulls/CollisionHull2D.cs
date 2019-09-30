using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : Particle2D
{
    public override void Start()
    {
        base.Start();
        PhysicsWorld.instance.addObject(this);
        
    }
    public abstract bool detectCollision(CollisionHull2D other);

    /*
     * VERIFIED WORKING
     */
    public static bool detectCollision(CircleHull2D lft, CircleHull2D rgt)
    {
        return lft.radius + rgt.radius >= Vector2.Distance(lft.position, rgt.position);
    }

    /*
     * VERIFIED WORKING
     */
    public static bool detectCollision(CircleHull2D circle, AABBHull square)
    {
        float closestPointX = Mathf.Max(square.position.x - square.halfLength.x, Mathf.Min(circle.position.x, square.position.x + square.halfLength.x));
        float closestPointY = Mathf.Max(square.position.y - square.halfLength.y, Mathf.Min(circle.position.y, square.position.y + square.halfLength.y));
        
        Vector2 closestPoint = new Vector2(closestPointX, closestPointY);
        Debug.DrawLine(new Vector3(closestPointX, closestPointY, 0), circle.position);
        return circle.radius + Vector2.Distance(closestPoint, square.position) > Vector2.Distance(circle.position, square.position);
    }

    public static bool detectCollision(CircleHull2D lft, OBBHull rgt)
    {
        Vector2 lftNorm = (lft.position - rgt.position).normalized;
        Vector2 oppositeNorm = Quaternion.Euler(0, 0, 90) * lftNorm;
        Vector4 projValues = getProjectionValuesOnNorm(rgt, lftNorm);
        float minR = Mathf.Min(new float[] { projValues.x, projValues.y, projValues.z, projValues.w });
        float maxR = Mathf.Max(new float[] { projValues.x, projValues.y, projValues.z, projValues.w });
        bool x = detectCollisionFromMinMax(new Vector2(minR, maxR), getProjectionValuesOnNorm(lft, lftNorm));
        projValues = getProjectionValuesOnNorm(rgt, oppositeNorm);
        minR = Mathf.Min(new float[] { projValues.x, projValues.y, projValues.z, projValues.w });
        maxR = Mathf.Max(new float[] { projValues.x, projValues.y, projValues.z, projValues.w });
        return x && detectCollisionFromMinMax(new Vector2(minR, maxR), getProjectionValuesOnNorm(lft, oppositeNorm));
    }

    /*
     * VERIFIED WORKING
     */
    public static bool detectCollision(AABBHull a, AABBHull b)
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

    public static bool detectCollisionFromMinMax(Vector2 aMinMax, Vector2 bMinMax)
    {
        if (aMinMax.x > bMinMax.x)
        {
            if (aMinMax.x < bMinMax.y)
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

    public static bool detectCollision(AABBHull lft, OBBHull rgt)
    {
        return false;//checkAxis(new Vector2(1,0), )
    }

    /*
     * Verified Working, possible error in using 4 checkaxis, i think i missed a check somewhere deeper in the code
     */
    public static bool detectCollision(OBBHull lft, OBBHull rgt)
    {
        //check each axis on each side, need to make better
        return checkAxis(lft, rgt, lft.getXNormal()) && checkAxis(lft, rgt, lft.getYNormal()) && checkAxis(lft, rgt, rgt.getXNormal()) && checkAxis(lft, rgt, rgt.getYNormal());
    }

    public static bool checkAxis(OBBHull a, OBBHull b, Vector2 norm)
    {
        Vector4 aProjValues = getProjectionValuesOnNorm(a, norm);
        Vector4 bProjValues = getProjectionValuesOnNorm(b, norm);
        float aMin = Mathf.Min(new float[] { aProjValues.x, aProjValues.y, aProjValues.z, aProjValues.w });
        float aMax = Mathf.Max(new float[] { aProjValues.x, aProjValues.y, aProjValues.z, aProjValues.w });
        float bMin = Mathf.Min(new float[] { bProjValues.x, bProjValues.y, bProjValues.z, bProjValues.w });
        float bMax = Mathf.Max(new float[] { bProjValues.x, bProjValues.y, bProjValues.z, bProjValues.w });
        Debug.DrawLine(aMin * norm, aMax * norm, Color.blue, 0.1f); //this is dumb useful, the projected line
        Debug.DrawLine(bMin * norm, bMax * norm, Color.red, 0.1f);//this is dumb useful, the projected line

        return detectCollisionFromMinMax(new Vector2(aMin, aMax), new Vector2(bMin, bMax));
    }
    public static Vector4 getProjectionValuesOnNorm(OBBHull box, Vector2 norm)
    {
        float a, b, c, d;
        int axis = norm.x != 0 ? 0 : 1; //just to make a scaler, but in case the x axis is zero, and if they are both zero then what the hell u doing with a normal (0,0)
        a = proj(box.getTopLeftPos(), norm)[axis] / norm[axis];
        b = proj(box.getTopRightPos(), norm)[axis] / norm[axis];
        c = proj(box.getBottomLeftPos(), norm)[axis] / norm[axis];
        d = proj(box.getBottomRightPos(), norm)[axis] / norm[axis];
        return new Vector4(a, b, c, d);
    }

    public static Vector2 getProjectionValuesOnNorm(CircleHull2D circle, Vector2 norm)
    {
        float a, b, c, d;
        int axis = norm.x != 0 ? 0 : 1; //just to make a scaler, but in case the x axis is zero, and if they are both zero then what the hell u doing with a normal (0,0)
        a = proj(circle.position, norm)[axis] - circle.radius/ norm[axis];
        b = proj(circle.position, norm)[axis] + circle.radius / norm[axis];
        return new Vector2(a, b);
    }

    public static Vector2 proj(Vector2 point, Vector2 normal)
    {
        return Vector2.Dot(point, normal) * normal;
    }

}