using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullCollision2D
{
    public HullCollision2D(CollisionHull2D a, CollisionHull2D b)
    {
        this.a = a;
        this.b = b;
    }

    public CollisionHull2D a;
    public CollisionHull2D b;
    public struct CollContact2D
    {
        public CollContact2D(CollisionHull2D a, CollisionHull2D b, Vector2 penetrationNorm)
        {
            hullA = a;
            hullB = b;
            this.penetrationNorm = a.velocity - b.velocity;
            restitution = (hullA.restitution+ hullB.restitution)/2.0f; //average
        }
        public CollisionHull2D hullA;
        public CollisionHull2D hullB;
        public float restitution;
        public Vector2 penetrationNorm;

        public void Resolve(float duration)
        {
            ResolveVelocity(duration);
            ResolveInterPenetration(duration);
        }

        public Vector2 calculateSeperatingVelocity()
        {
            Vector2 relativeVelocity = hullA.getVelocity();
            relativeVelocity -= hullB.getVelocity();
            return relativeVelocity * penetrationNorm;
        }

        private void ResolveVelocity(float duration)
        {
            Vector2 seperatingVelocity = calculateSeperatingVelocity();
            if(seperatingVelocity.magnitude > 0)
            {
                return; //they are already seperating
            }

            Vector2 newSeperatingVelocity = -seperatingVelocity * restitution;
            Vector2 deltaVelocity = newSeperatingVelocity - seperatingVelocity;

            float totalInverseMass = hullA.InverseMass + hullB.InverseMass;
            if(totalInverseMass <= 0)
            {
                return; //dont do anything, they cannot move
            }

            Vector2 inpulse = deltaVelocity / totalInverseMass;
            Vector2 inpulsePerMass = penetrationNorm.normalized * inpulse.magnitude * newSeperatingVelocity;
            float isHullRight = 1.0f;
            if(Vector2.Dot(hullA.getVelocity(), penetrationNorm) > 0.0f)
            {
                isHullRight = -1.0f;
            }
            hullA.setVelocity(hullA.getVelocity() + inpulsePerMass * -hullB.InverseMass * isHullRight);
            if (Vector2.Dot(hullB.getVelocity(), penetrationNorm) > 0.0f)
            {
                isHullRight = -1.0f;
            }
            hullB.setVelocity(hullB.getVelocity() + inpulsePerMass * -hullA.InverseMass * isHullRight);
        }

        private void ResolveInterPenetration(float duration)
        {
            if(penetrationNorm.magnitude <= 0)
            {
                return;
            }

            float totalInverseMass = hullA.InverseMass + hullB.InverseMass;
            if (totalInverseMass <= 0)
            {
                return; //dont do anything, they cannot move
            }

            Vector2 movePerMass = penetrationNorm * (penetrationNorm.magnitude / totalInverseMass);

            Vector2 hullAMovement = movePerMass * hullA.InverseMass;
            Vector2 hullBMovement = movePerMass * hullB.InverseMass;

            hullA.position = hullA.position + hullAMovement;
            hullB.position = hullB.position + hullBMovement;
        }

    }
    public CollContact2D[] contactPoints;

    public void Resolve(float duration)
    {
        for(int i = 0; i < contactPoints.Length; i++)
        {
            contactPoints[i].Resolve(duration);
        }
    }


}
