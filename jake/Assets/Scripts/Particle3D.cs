using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    public enum PhysicsType
    {
        Kinematic,
        Euler
    }

    public float m_StartMass;
    public Vector3 m_Position;
    public Vector3 m_Velocity;
    public Vector3 m_Acceleration;
    public Quaternion m_Rotation;
    public Vector3 m_Torque;
    public Vector3 m_AngularVelocity;
    public Vector3 m_AngularAcceleration;
    public Vector3 inertiaBounds = new Vector3(1.0f, 1.0f, 1.0f); //assume a 1x1x1 cube
    protected float m_Inertia;


    public PhysicsType m_CalculationType;

    protected float m_InverseMass;
    public float Mass
    {
        set { m_InverseMass = value > 0.0f ? 1.0f / value : 0.0f; }
        get { return 1 / m_InverseMass; }
    }

    public int physicsIterations = 2;

    public void Start()
    {
        Mass = m_StartMass;
        m_Inertia = CalculateInertia();
    }

    public float CalculateInertia()
    {
        return 1.0f;
    }

    public void FixedUpdate()
    {
        m_Position = transform.position;
        m_Rotation = transform.rotation;

        m_AngularVelocity += m_AngularAcceleration * Time.fixedDeltaTime;
        if(m_CalculationType == PhysicsType.Euler)
        {
            updatePositionEulerExplicit(Time.fixedDeltaTime);
            updateRotationEulerExplicit(Time.fixedDeltaTime);
        }
        else
        {
            updatePositionKinematic(Time.fixedDeltaTime);
            updateRotationKinematic(Time.fixedDeltaTime);
        }
        //m_AngularAcceleration = Vector3.zero;
        //updateAngularAcceleration();

        transform.position = m_Position;
        transform.rotation = m_Rotation;
    }

    public virtual void OnDrawGizmos()
    {
        Vector3 offset = Vector3.zero;
        if(Application.isPlaying)
        {
            offset = transform.position;
        }
        Matrix4x4 oldMat = Gizmos.matrix;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(m_Position, m_Rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = Color.white;
        Gizmos.DrawCube(m_Position, new Vector3(1.0f, 1.0f, 1.0f));
        Gizmos.matrix = oldMat;
        Gizmos.DrawRay(m_Position, m_Rotation * Vector3.forward);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(m_Position, Vector3.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(m_Position, Vector3.right);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(m_Position, Vector3.up);
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
        //rotation = rotation + angularVelocity * deltaTime;
        //angularVelocity += angularAcceleration * deltaTime;
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
        //rotation = rotation + (angularVelocity * deltaTime) + (0.5f * angularAcceleration * deltaTime * deltaTime);
        //angularVelocity += angularAcceleration * deltaTime;

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

    void updateAngularAcceleration()
    {
        if (m_InverseMass == 0.0f)
            return;
        m_AngularAcceleration = (m_Torque / m_Inertia);
        m_Torque = Vector3.zero;
    }
}
