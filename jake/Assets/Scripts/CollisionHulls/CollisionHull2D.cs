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
    public static bool detectCollision(CircleHull2D circle, AABBHull2D square)
    {
        float closestPointX = Mathf.Clamp(circle.position.x, square.position.x - square.halfLength.x, square.position.x + square.halfLength.x);
        float closestPointY = Mathf.Clamp(circle.position.y, square.position.y - square.halfLength.y, square.position.y + square.halfLength.y);

        Vector2 closestPoint = new Vector2(closestPointX, closestPointY);
        Debug.DrawLine(new Vector3(closestPointX, closestPointY, 0), circle.position);
        return circle.radius > Vector2.Distance(circle.position, closestPoint);
    }

    public static bool detectCollision(CircleHull2D circle, OBBHull2D square)
    {
        Vector2 closestPoint = square.ClosestPointTo(circle.position);
        Debug.DrawLine(new Vector3(closestPoint.x, closestPoint.y, 0), circle.position);
        return circle.radius > Vector2.Distance(circle.position, closestPoint);
    }

    /*
     * VERIFIED WORKING
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

    /*
    * VERIFIED WORKING, based off OBB v OBB Collision
    */
    public static bool detectCollision(OBBHull2D lft, AABBHull2D rgt)
    {
        return checkAxis(lft, rgt, lft.getXNormal()) && 
            checkAxis(lft, rgt, lft.getYNormal()) && 
            checkAxis(lft, rgt, Vector2.left) && 
            checkAxis(lft, rgt, Vector2.up);
    }

    /*
     * Verified Working, possible error in using 4 checkaxis, i think i missed a check somewhere deeper in the code
     */
    public static bool detectCollision(OBBHull2D lft, OBBHull2D rgt)
    {
        //check each axis on each side, need to make better
        return checkAxis(lft, rgt, lft.getXNormal()) && 
            checkAxis(lft, rgt, lft.getYNormal()) && 
            checkAxis(lft, rgt, rgt.getXNormal()) && 
            checkAxis(lft, rgt, rgt.getYNormal());
    }

    public static bool checkAxis(OBBHull2D a, OBBHull2D b, Vector2 norm)
    {
        Vector2 aProjValues = getMinMaxProjectionValuesOnNorm(a, norm);
        Vector2 bProjValues = getMinMaxProjectionValuesOnNorm(b, norm);
        Debug.DrawLine(aProjValues.x * norm, aProjValues.y * norm, Color.blue); //this is dumb useful, the projected line
        Debug.DrawLine(bProjValues.x * norm, bProjValues.y * norm, Color.red);//this is dumb useful, the projected line

        return detectCollisionFromMinMax(aProjValues, bProjValues);
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
        float a, b, c, d;
        int axis = norm.x != 0 ? 0 : 1; //just to make a scaler, but in case the x axis is zero, and if they are both zero then what the hell u doing with a normal (0,0)
        a = (proj(circle.position, norm)[axis] - circle.radius)/ norm[axis];
        b = (proj(circle.position, norm)[axis] + circle.radius)/ norm[axis];
        return new Vector2(a, b);
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

    public static Vector2 proj(Vector2 point, Vector2 normal)
    {
        return Vector2.Dot(point, normal) * normal;
    }
}