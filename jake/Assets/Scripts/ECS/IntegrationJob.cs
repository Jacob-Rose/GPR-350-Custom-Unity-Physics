using System.Collections;
using System.Collections.Generic;
using UnityEngine.Jobs;
using UnityEngine;

public struct IntegrationJob : IJobParallelForTransform
{
    public Vector3 m_Position;
    public Vector3 m_Velocity;
    public Vector3 m_Acceleration;
    public Quaternion m_Rotation;
    public Vector3 m_AngularVelocity;
    public Vector3 m_AngularAcceleration;

    public void Execute(int index, TransformAccess transform)
    {
        transform.position = m_Position;
    }

    void updatePositionEulerExplicit(float deltaTime)
    {
        m_Position += m_Velocity * deltaTime;
        m_Velocity += m_Acceleration * deltaTime;
    }

    void updatePositionKinematic(float deltaTime)
    {
        m_Position += (m_Velocity * deltaTime) + (0.5f * m_Acceleration * deltaTime * deltaTime);
        m_Velocity += m_Acceleration * deltaTime;
    }

    void updateRotationEulerExplicit(float deltaTime)
    {
        Quaternion angularVelocityQuaternion = new Quaternion(m_AngularVelocity.x, m_AngularVelocity.y, m_AngularVelocity.z, 0);
        Quaternion deltaRotation = angularVelocityQuaternion * m_Rotation;
        Quaternion newRotation = new Quaternion(m_Rotation.x + (deltaRotation.x * deltaTime * 0.5f),
            m_Rotation.y + (deltaRotation.y * deltaTime * 0.5f),
            m_Rotation.z + (deltaRotation.z * deltaTime * 0.5f),
            m_Rotation.w + (deltaRotation.w * deltaTime * 0.5f));
        m_Rotation = newRotation.normalized;
    }

    void updateRotationKinematic(float deltaTime)
    {
        Quaternion angularVelocityQuaternion = new Quaternion(m_AngularVelocity.x * deltaTime, m_AngularVelocity.y * deltaTime, m_AngularVelocity.z * deltaTime, 0);
        Quaternion deltaRotation = angularVelocityQuaternion * m_Rotation;
        float deltaSquaredO2 = deltaTime * deltaTime * 0.5f; //delta squared over two
        Vector4 accelerationDeltaRotation = new Vector4(m_AngularAcceleration.x * deltaSquaredO2,
            m_AngularAcceleration.y * deltaSquaredO2,
            m_AngularAcceleration.z * deltaSquaredO2, 0.0f);
        Quaternion newRotation = new Quaternion(m_Rotation.x + deltaRotation.x + accelerationDeltaRotation.x,
            m_Rotation.y + deltaRotation.y + accelerationDeltaRotation.y,
            m_Rotation.z + deltaRotation.z + accelerationDeltaRotation.z,
            m_Rotation.w + deltaRotation.w + accelerationDeltaRotation.w);
        m_Rotation = newRotation.normalized;
    }
}
