using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class Particle2DTest : MonoBehaviour
{
    public TestType testType;
    public enum TestType
    {
        Gravity,
        Normal,
        Sliding,
        StaticFriction,
        KineticFriction,
        Drag,
        Spring,
        Torque
    }

    public Vector2 variable1; //surface norm for some, point of force adding for torque
    public Vector2 variable2; //force used for torque equation
    public void FixedUpdate()
    {
        Particle2D comp = GetComponent<Particle2D>();
        if (testType == TestType.Gravity)
        {
            comp.addForce(ForceGenerator2D.GenerateForce_Gravity(comp.Mass, -9.81f, Vector2.up));
        }
        else if (testType == TestType.Normal)
        {
            comp.addForce(ForceGenerator2D.GenerateForce_Normal(new Vector2(0.0f, -9.81f), variable1));
        }
        else if (testType == TestType.Sliding)
        {
            comp.addForce(ForceGenerator2D.GenerateForce_Sliding(new Vector2(0.0f, -9.81f), variable1));
        }
        else if (testType == TestType.StaticFriction)
        {
            comp.addForce(ForceGenerator2D.GenerateForce_Sliding(new Vector2(0.0f, -9.81f), variable1));
            comp.addForce(ForceGenerator2D.GenerateForce_Friction_Static(variable1, -variable1, 0.8f));
        }
        else if (testType == TestType.KineticFriction)
        {
            comp.addForce(ForceGenerator2D.GenerateForce_Sliding(new Vector2(0.0f, -9.81f), variable1));
            comp.addForce(ForceGenerator2D.GenerateForce_Friction_Kinetic(variable1, comp.getVelocity(), 0.8f));
        }
        else if (testType == TestType.Drag)
        {
            //gravity for testing also applied
            comp.addForce(ForceGenerator2D.GenerateForce_Gravity(comp.Mass, -9.81f, Vector2.up));
            comp.addForce(ForceGenerator2D.GenerateForce_Drag(comp.getVelocity(), Vector2.zero, 0.2f, 100, 0.9f));
        }
        else if (testType == TestType.Spring)
        {
            //gravity for testing also applied
            comp.addForce(ForceGenerator2D.GenerateForce_Gravity(comp.Mass, -9.81f, Vector2.up));
            comp.addForce(ForceGenerator2D.GenerateForce_Spring(comp.position, Vector2.zero, -4.0f, 10.0f));
        }
        else if (testType == TestType.Torque)
        {
            comp.addForceAtPoint(variable1, variable2);
        }
    }
}
