using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator3D
{
    public static Vector3 GenerateForce_Gravity(float particleMass, float gravitationalConst, Vector3 worldUp)
    {
        return worldUp * gravitationalConst * particleMass;
    }

    public static Vector3 GenerateForce_Normal(Vector3 f_gravity, Vector3 surfaceNormal)
    {
        return Vector3.Project(-f_gravity, surfaceNormal.normalized);
    }

    public static Vector3 GenerateForce_Friction_Static(Vector3 f_normal, Vector3 f_opposing, float frictionCoefficient)
    {
        return frictionCoefficient * f_normal.magnitude > f_opposing.magnitude ? -frictionCoefficient * f_normal : -f_opposing;
    }
}
