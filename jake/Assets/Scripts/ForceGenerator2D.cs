using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator2D
{
    public static Vector2 GenerateForce_Gravity(float particleMass, float gravitationalConst, Vector2 worldUp)
    {
        return worldUp * gravitationalConst * particleMass;
    }
    public static Vector2 GenerateForce_Normal(Vector2 f_gravity, Vector2 surfaceNormal)
    {
        return Vector3.Project(f_gravity, surfaceNormal.normalized);
    }
    public static Vector2 GenerateForce_Sliding(Vector2 f_gravity, Vector2 f_normal)
    {
        return f_gravity + f_normal;
    }
    public static Vector2 GenerateForce_Friction_Static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient)
    {
        return frictionCoefficient * f_normal.magnitude > f_opposing.magnitude ? -frictionCoefficient * f_normal : -f_opposing;
    }

    public static Vector2 GenerateForce_Friction_Kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient)
    {
        return -frictionCoefficient * f_normal.magnitude * particleVelocity.normalized;
    }

    public static Vector2 GenerateForce_Drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        //todo wrong
        return (0.5f * (particleVelocity - fluidVelocity) * (particleVelocity - fluidVelocity) * fluidDensity * objectArea_crossSection * objectDragCoefficient);
    }

    public static Vector2 GenerateForce_Spring(Vector2 particlePos, Vector2 anchorPos, float springRestingLength, float springStiffnessCoefficient)
    {
        return -springStiffnessCoefficient * (Vector2.Distance(particlePos, anchorPos) - springRestingLength) * (particlePos - anchorPos).normalized;
    }
}