using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ParticleComponentData : IComponentData
{
    public Vector3 m_Position;
    public Vector3 m_Velocity;
    public Vector3 m_Acceleration;

    public Vector3 m_Force;

    public float m_InverseMass { get; private set; }

    public float Mass
    {
        set { m_InverseMass = value > 0.0f ? 1.0f / value : 0.0f; }
        get { return 1 / m_InverseMass; }
    }
    public void updatePositionEulerExplicit(float deltaTime)
    {
        m_Position += m_Velocity * deltaTime;
        m_Velocity += m_Acceleration * deltaTime;
    }

    public void updatePositionKinematic(float deltaTime)
    {
        m_Position += (m_Velocity * deltaTime) + (0.5f * m_Acceleration * deltaTime * deltaTime);
        m_Velocity += m_Acceleration * deltaTime;
    }


    public void Update()
    {
        updateAcceleration();
        if (true) //set to false to change how they all run
        {
            updatePositionEulerExplicit(Time.deltaTime);
        }
        else
        {
            updatePositionKinematic(Time.deltaTime);
        }
    }

    public void updateAcceleration()
    {
        m_Acceleration = m_Force * m_InverseMass;
        m_Force = Vector3.zero;
    }
}
