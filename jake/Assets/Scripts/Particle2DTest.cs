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
        Spring
    }
    public void FixedUpdate()
    {
        Particle2D comp = GetComponent<Particle2D>();
        if (testType == TestType.Gravity)
        {
            comp.addForce(ForceGenerator2D.GenerateForce_Gravity(comp.Mass, -9.81f, Vector2.up));
        }
        else if (testType == TestType.Normal)
        {
            Vector2 surfNorm = new Vector2(0.0f, 1.0f);
            comp.addForce(ForceGenerator2D.GenerateForce_Normal(new Vector2(0.0f, -9.81f), surfNorm));
        }
        else if (testType == TestType.Sliding)
        {
            Vector2 surf = new Vector2(4.0f, 1.0f);
            comp.addForce(ForceGenerator2D.GenerateForce_Sliding(new Vector2(0.0f, -9.81f), surf));
        }
        else if (testType == TestType.StaticFriction)
        {
            Vector2 surf = new Vector2(4.0f, 1.0f);
            comp.addForce(ForceGenerator2D.GenerateForce_Sliding(new Vector2(0.0f, -9.81f), surf));
            comp.addForce(ForceGenerator2D.GenerateForce_Friction_Static(surf, -surf, 0.8f));
        }
        else if (testType == TestType.KineticFriction)
        {
            Vector2 surf = new Vector2(4.0f, 1.0f);
            comp.addForce(ForceGenerator2D.GenerateForce_Sliding(new Vector2(0.0f, -9.81f), surf));
            comp.addForce(ForceGenerator2D.GenerateForce_Friction_Kinetic(surf, comp.getVelocity(), 0.8f));
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
            comp.addForce(ForceGenerator2D.GenerateForce_Spring(comp.position, Vector2.zero, 2.0f, 100.0f));
        }
    }
}
