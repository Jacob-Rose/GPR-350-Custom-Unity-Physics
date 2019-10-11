using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullCollision2D
{
    public HullCollision2D(CollisionHull2D a, CollisionHull2D b, Vector2 contactNormal, float penetration)
    {
        this.a = a;
        this.b = b;
        this.restitution = Mathf.Min(a.restitution, b.restitution);
        this.contactNormal = contactNormal;
        this.penetration = penetration;
    }

    public HullCollision2D(CollisionHull2D a, CollisionHull2D b)
    {
        this.a = a;
        this.b = b;
        this.restitution = Mathf.Min(a.restitution, b.restitution);
        this.contactNormal = a.position - b.position;
        this.penetration = 0;
    }

    public CollisionHull2D a;
    public CollisionHull2D b;
    public float penetration;
    public float restitution;
    public Vector2 contactNormal;

    public void Resolve()
    {
        ResolveVelocity();
        ResolveInterPenetration();
    }

    public float calculateSeperatingVelocity()
    {
        Vector2 relativeVelocity = a.getVelocity();
        relativeVelocity -= b.getVelocity();
        return Vector2.Dot(relativeVelocity, contactNormal);
    }

    private void ResolveVelocity()
    {
        float seperatingVelocity = calculateSeperatingVelocity();
        if (seperatingVelocity > 0)
        {
            return; //they are already seperating
        }

        float newSeperatingVelocity = -seperatingVelocity * restitution;
        float deltaVelocity = newSeperatingVelocity - seperatingVelocity;

        float totalInverseMass = a.InverseMass + b.InverseMass;
        if (totalInverseMass <= 0)
         {
            return; //dont do anything, they cannot move
        }

        float inpulse = deltaVelocity / totalInverseMass;
        Vector2 inpulsePerMass = contactNormal * inpulse * newSeperatingVelocity;
        /*if(Vector2.Dot(seperatingVelocity, penetrationNorm) > 0.0f)
        {
            isHullRight = -1.0f;
        }*/
        a.setVelocity(a.getVelocity() + inpulsePerMass * -a.InverseMass * -1.0f);
        /*if (Vector2.Dot(seperatingVelocity, penetrationNorm) > 0.0f)
        {
            isHullRight = -1.0f;
        }*/
        b.setVelocity(b.getVelocity() + inpulsePerMass * -b.InverseMass);
    }

    private void ResolveInterPenetration()
    {
        if (penetration <= 0)
        {
            return;
        }

        float totalInverseMass = a.InverseMass + b.InverseMass;
        if (totalInverseMass <= 0)
        {
            return; //dont do anything, they cannot move
        }

        Vector2 movePerMass = contactNormal * (penetration / totalInverseMass);
        Vector2 hullAMovement = movePerMass * a.InverseMass;
        Vector2 hullBMovement = movePerMass * -b.InverseMass;
        

        a.position = a.position + hullAMovement;
        b.position = b.position + hullBMovement;

    }


}
