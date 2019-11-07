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
    public Vector2 contactPoint = Vector2.zero;

    public void Resolve(float deltaT)
    {
        a.transform.position = a.position;
        b.transform.position = b.position;
        ResolveVelocity(deltaT);
        
        ResolveInterPenetration();
    }

    public float calculateSeperatingVelocity()
    {
        Vector2 relativeVelocity = a.getVelocity();
        relativeVelocity -= b.getVelocity();
        return Vector2.Dot(relativeVelocity, contactNormal);
    }

    private void ResolveVelocity(float deltaT)
    {
        float seperatingVelocity = calculateSeperatingVelocity();
        if (seperatingVelocity > 0)
        {
            return; //they are already seperating
        }

        float newSeperatingVelocity = -seperatingVelocity * restitution;
        
        Vector2 accCausedVelocity = b.acceleration - a.acceleration;
        float accCausedSepVelocity = Vector2.Dot(accCausedVelocity, contactNormal) * deltaT;

        if(accCausedSepVelocity> 0)
        {
            newSeperatingVelocity += restitution * accCausedSepVelocity;

            if(newSeperatingVelocity < 0)
            {
                newSeperatingVelocity = 0.0f;
            }
        }

        float deltaVelocity = newSeperatingVelocity - seperatingVelocity;

        float totalInverseMass = a.InverseMass + b.InverseMass;
        if (totalInverseMass <= 0)
         {
            return; //dont do anything, they cannot move
        }

        float inpulse = deltaVelocity / totalInverseMass;
        Vector2 inpulsePerMass = contactNormal * inpulse * newSeperatingVelocity;
        a.setVelocity(a.getVelocity() + (inpulsePerMass * a.InverseMass));
        b.setVelocity(b.getVelocity() + (inpulsePerMass * -b.InverseMass));
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
