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

    public static bool detectCollision(CircleHull2D lft, CircleHull2D rgt)
    {
        //VERIFIED WORKING
        return lft.radius + rgt.radius >= Vector2.Distance(lft.position, rgt.position);
    }

    public static bool detectCollision(CircleHull2D lft, AABBHull rgt)
    {
        float closestPointX = Mathf.Max(rgt.position.x, Mathf.Min(lft.position.x, rgt.position.x + rgt.halfLength.x));
        float closestPointY = Mathf.Max(rgt.position.y, Mathf.Min(lft.position.y, rgt.position.y + rgt.halfLength.y));
        Vector2 closestPoint = new Vector2(closestPointX, closestPointY);
        return lft.radius + Vector2.Distance(closestPoint, rgt.position) > Vector2.Distance(lft.position, rgt.position);
    }

    public static bool detectCollision(CircleHull2D lft, OBBHull rgt)
    {
        //float closestPointX = Mathf.Max(rgt.position.x, Mathf.Min(lft.position.x, rgt.position.x + rgt.halfLength.x));
        //float closestPointY = Mathf.Max(rgt.position.y, Mathf.Min(lft.position.y, rgt.position.y + rgt.halfLength.y));
        return false;
    }

    public static bool detectCollision(AABBHull lft, AABBHull rgt)
    {
        //VERIFIED WORKING
        return lft.position.x + lft.halfLength.x > rgt.position.x - rgt.halfLength.x && 
            lft.position.y + lft.halfLength.y > rgt.position.y - rgt.halfLength.y;
    }

    public static bool detectCollision(AABBHull lft, OBBHull rgt)
    {
        return false;//checkAxis(new Vector2(1,0), )
    }

    public static bool detectCollision(OBBHull lft, OBBHull rgt)
    {
        //close, doesnt work on Y axis
        return checkAxis(lft.getXNormal(), rgt, lft) && checkAxis(lft.getYNormal(), rgt, lft);
    }

    public static bool checkAxis(Vector2 norm, OBBHull other, OBBHull me)
    {
        Vector4 minMaxOther = getMaxAndMinFromProj(norm, other);
        Vector4 minMaxMe = getMaxAndMinFromProj(norm, me);
        Vector2 minMe = new Vector2(minMaxMe.x, minMaxMe.y);
        Vector2 maxOther = new Vector2(minMaxOther.z, minMaxOther.w);
        Debug.DrawLine(new Vector3(minMaxOther.x, minMaxOther.y, 0), new Vector3(minMaxOther.z, minMaxOther.w), Color.blue, 0.1f);
        Debug.DrawLine(new Vector3(minMaxMe.x, minMaxMe.y, 0), new Vector3(minMaxMe.z, minMaxMe.w), Color.red, 0.1f);
        return minMe.x > maxOther.x && minMe.y > maxOther.y;
    }

    public static Vector4 getMaxAndMinFromProj(Vector2 norm, OBBHull box)
    {
        Vector2 projTL = proj(box.getTopLeftPos(), norm);
        Vector2 projTR = proj(box.getTopRightPos(), norm);
        Vector2 projBL = proj(box.getBottomLeftPos(), norm);
        Vector2 projBR = proj(box.getBottomRightPos(), norm);
        float maxXother = Mathf.Max(new float[] { projTL.x, projTR.x, projBL.x, projBR.x });
        float maxYother = Mathf.Max(new float[] { projTL.y, projTR.y, projBL.y, projBR.y });
        float minXother = Mathf.Min(new float[] { projTL.x, projTR.x, projBL.x, projBR.x });
        float minYother = Mathf.Min(new float[] { projTL.y, projTR.y, projBL.y, projBR.y });

        return new Vector4(maxXother, maxYother, minXother, minYother);
    }

    public static Vector2 proj(Vector2 point, Vector2 normal)
    {
        return Vector2.Dot(point, normal) * normal;
    }

}