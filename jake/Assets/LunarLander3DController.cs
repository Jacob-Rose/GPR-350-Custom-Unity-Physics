using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarLander3DController : OBBHull3D
{
    public Transform m_ThrusterCenter;
    public float m_ThrusterDistFromCenter;

    public float m_ThrusterForcePerSecond;

    // Update is called once per frame
    public override void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        AddForce(ForceGenerator3D.GenerateForce_Gravity(Mass, -9.8f, Vector3.up) * deltaTime);
        if(Input.GetKey(KeyCode.A))
        {
            AddThrust(deltaTime, new Vector3(m_ThrusterDistFromCenter, 0));
        }
        if(Input.GetKey(KeyCode.D))
        {
            AddThrust(deltaTime, new Vector3(-m_ThrusterDistFromCenter, 0));
        }
        if(Input.GetKey(KeyCode.W))
        {
            AddThrust(deltaTime, new Vector3(0, 0, m_ThrusterDistFromCenter));
        }
        if(Input.GetKey(KeyCode.S))
        {
            AddThrust(deltaTime, new Vector3(0, 0, -m_ThrusterDistFromCenter));
        }
        if(Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(m_ThrusterDistFromCenter, 0));
            AddThrust(deltaTime, new Vector3(-m_ThrusterDistFromCenter, 0));
            AddThrust(deltaTime, new Vector3(0, 0, m_ThrusterDistFromCenter));
            AddThrust(deltaTime, new Vector3(0, 0, -m_ThrusterDistFromCenter));
        }
        
        base.FixedUpdate();
    }


    void AddThrust(float deltaTime, Vector3 offset)
    {
        AddForceAtPointLocal(new Vector3(0, m_ThrusterForcePerSecond * deltaTime, 0), offset);
    }
}
