using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3DTest : Particle3D
{
    public override void FixedUpdate()
    {
        Vector3 surfNorm = new Vector3(0.1f, 1.0f, 0.0f).normalized;
        Debug.DrawRay(Vector3.zero, surfNorm);
        Vector3 gravForce = ForceGenerator3D.GenerateForce_Gravity(Mass, -9.81f, Vector3.up);
        AddForce(gravForce);
        Vector3 normForce = ForceGenerator3D.GenerateForce_Normal(gravForce, surfNorm);
        AddForce(normForce);
        base.FixedUpdate();
    }
}
