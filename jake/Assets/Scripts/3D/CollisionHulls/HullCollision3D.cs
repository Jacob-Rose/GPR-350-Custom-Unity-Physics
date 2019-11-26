using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullCollision3D
{
    public CollisionHull3D a;
    public CollisionHull3D b;
    public float penetration;
    public float restitution;
    public Vector3 contactNormal;
    public Vector3 contactPoint = Vector2.zero;
    // Start is called before the first frame update
    public HullCollision3D(CollisionHull3D a, CollisionHull3D b, Vector3 contactNormal, float penetration, Vector3[] contactPoint)
    {
        this.a = a;
        this.b = b;
        restitution = Mathf.Min(a.m_Restitution, b.m_Restitution);
        this.contactNormal = contactNormal;
        this.penetration = penetration;
        this.contactPoint = contactPoint[0];
    }

    public void Resolve(float deltaT)
    {
        a.transform.position = a.m_Position;
        b.transform.position = b.m_Position;

        ResolveVelocity(deltaT);
        ResolveInterPenetration();

    }

    public float calculateSeperatingVelocity()
    {
        Vector3 relativeVelocity = a.m_Velocity;
        relativeVelocity -= b.m_Velocity;
        return Vector3.Dot(relativeVelocity, contactNormal);
    }

    private void ResolveAngularVelocity(float deltaT)
    {
        if(a is OBBHull3D || b is OBBHull3D)
        {

        }
    }

    private void ResolveVelocity(float deltaT)
    {
        float seperatingVelocity = calculateSeperatingVelocity();
        if (seperatingVelocity > 0)
        {
            return; //they are already seperating
        }

        float newSeperatingVelocity = -seperatingVelocity * restitution;

        Vector3 accCausedVelocity = b.m_Acceleration - a.m_Acceleration;
        float accCausedSepVelocity = Vector3.Dot(accCausedVelocity, contactNormal) * deltaT;

        if (accCausedSepVelocity > 0)
        {
            newSeperatingVelocity += restitution * accCausedSepVelocity;

            if (newSeperatingVelocity < 0)
            {
                newSeperatingVelocity = 0.0f;
            }
        }

        float deltaVelocity = newSeperatingVelocity - seperatingVelocity;

        float totalInverseMass = a.m_InverseMass + b.m_InverseMass;
        if (totalInverseMass <= 0)
        {
            return; //dont do anything, they cannot move
        }

        float inpulse = deltaVelocity / totalInverseMass;
        Vector3 inpulsePerMass = contactNormal * inpulse * newSeperatingVelocity;
        a.m_Velocity = a.m_Velocity + (inpulsePerMass * a.m_InverseMass);
        b.m_Velocity = b.m_Velocity + (inpulsePerMass * -b.m_InverseMass);
    }

    private void ResolveInterPenetration()
    {
        if (penetration <= 0) { return; }

        float totalInverseMass = a.m_InverseMass + b.m_InverseMass;
        if (totalInverseMass <= 0)
        {
            return; //dont do anything, they cannot move
        }

        Vector3 movePerMass = contactNormal.normalized * (penetration * 2.0f / totalInverseMass);

        Vector3 hullAMovement = movePerMass * a.m_InverseMass;
        Vector3 hullBMovement = movePerMass * -b.m_InverseMass;

        a.m_Position = a.m_Position + hullAMovement;
        b.m_Position = b.m_Position + hullBMovement;

    }


}
