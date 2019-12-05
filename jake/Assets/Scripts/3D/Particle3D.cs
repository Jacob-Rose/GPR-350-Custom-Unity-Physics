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

    public enum Shape
    {
        SolidSphere,
        HollowSphere,
        SolidBox,
        HollowBox,
        SolidCylinder,
        SolidCone
    }

    public float m_StartMass;
    public Vector3 m_Position;
    public Vector3 m_Velocity;
    public Vector3 m_Acceleration;
    public Quaternion m_Rotation;
    public Vector3 m_CenterOfMass = Vector3.zero;
    private Vector3 m_CenterOfMassWorld;
    public Vector3 m_Torque;
    public Vector3 m_AngularVelocity;
    public Vector3 m_AngularAcceleration;
    public PhysicsType m_CalculationType;

    public Shape m_Shape;
    public Vector3 m_SizeA;//use only x for dist

    public Vector3 m_Force = Vector3.zero;

    protected Matrix4x4 m_InvInertiaLocalSpace;
    protected Matrix4x4 m_InvInertiaWorldSpace;

    protected Matrix4x4 m_ObjectToWorldTransform;
    protected Matrix4x4 m_WorldToObjectTransform;

    public Vector3 centerOfMassLocalSpace = Vector3.zero;

    public float m_InverseMass { get; private set; }
    public float Mass
    {
        set { m_InverseMass = value > 0.0f ? 1.0f / value : 0.0f; }
        get { return 1 / m_InverseMass; }
    }

    public virtual void Start()
    {
        Mass = m_StartMass;
        m_InvInertiaLocalSpace = getInertiaTensorMatrix().inverse;
        transform.position += m_Position;
        m_Position = transform.position + new Vector3(m_Position.x, m_Position.y, 0);
        m_Rotation = transform.rotation;
        m_ObjectToWorldTransform = Matrix4x4.TRS(m_Position, m_Rotation, Vector3.one);
        m_WorldToObjectTransform = m_ObjectToWorldTransform.transpose;
    }

    public Matrix4x4 getInertiaTensorMatrix()
    {
        if(m_Shape == Shape.SolidSphere)
        {
            float inertia = 0.4f * Mass * m_SizeA.x * m_SizeA.x;
            return new Matrix4x4(
                new Vector4(inertia, 0, 0, 0),
                new Vector4(0, inertia, 0, 0),
                new Vector4(0, 0, inertia, 0),
                new Vector4(0, 0, 0, 1));
        }
        else if(m_Shape == Shape.HollowSphere)
        {
            float inertia = 0.666f * Mass * m_SizeA.x * m_SizeA.x;
            return new Matrix4x4(
                new Vector4(inertia, 0, 0, 0),
                new Vector4(0, inertia, 0, 0),
                new Vector4(0, 0, inertia, 0),
                new Vector4(0, 0, 0, 1));
        }
        else if(m_Shape == Shape.SolidBox)
        {
            return new Matrix4x4(
                new Vector4(0.0833f * Mass * (m_SizeA.y*m_SizeA.y + m_SizeA.z*m_SizeA.z), 0, 0, 0), //yz
                new Vector4(0, 0.0833f * Mass * (m_SizeA.x * m_SizeA.x + m_SizeA.z * m_SizeA.z), 0, 0), //xz
                new Vector4(0, 0, 0.0833f * Mass * (m_SizeA.y * m_SizeA.y + m_SizeA.x * m_SizeA.x), 0), //xy
                new Vector4(0, 0, 0, 1));
        }
        else if(m_Shape == Shape.HollowBox)
        {
            return new Matrix4x4(
               new Vector4(1.666f * Mass * (m_SizeA.y * m_SizeA.y + m_SizeA.z * m_SizeA.z), 0, 0, 0), //yz
               new Vector4(0, 1.666f * Mass * (m_SizeA.x * m_SizeA.x + m_SizeA.z * m_SizeA.z), 0, 0), //xz
               new Vector4(0, 0, 1.666f * Mass * (m_SizeA.y * m_SizeA.y + m_SizeA.x * m_SizeA.x), 0), //xy
               new Vector4(0, 0, 0, 1));
        }
        else if(m_Shape == Shape.SolidCylinder)
        {
            return new Matrix4x4(
                new Vector4(0.0833f * Mass * (3 * m_SizeA.x * m_SizeA.x + m_SizeA.y * m_SizeA.y), 0, 0, 0), //yz
                new Vector4(0, 0.0833f * Mass * (3 * m_SizeA.x * m_SizeA.x + m_SizeA.y * m_SizeA.y), 0, 0), //xz
                new Vector4(0, 0, 0.0833f * Mass * (m_SizeA.x * m_SizeA.x), 0), //xy
                new Vector4(0, 0, 0, 1));
        }
        else if(m_Shape == Shape.SolidCone)
        {
            return new Matrix4x4(
                new Vector4((0.6f * Mass * m_SizeA.y * m_SizeA.y) + (0.15f * Mass * m_SizeA.x * m_SizeA.x), 0, 0, 0), //yz
                new Vector4(0, (0.6f * Mass * m_SizeA.y * m_SizeA.y) + (0.15f * Mass * m_SizeA.x * m_SizeA.x), 0, 0), //xz
                new Vector4(0, 0, 0.3f * Mass * m_SizeA.x * m_SizeA.x, 0), //xy
                new Vector4(0, 0, 0, 1));
        }
        throw new System.Exception("Shape inertia calculation not found");
    }
    void updateAngularAcceleration()
    {
        if (m_InverseMass == 0.0f)
            return;
        m_AngularAcceleration = m_InvInertiaLocalSpace * m_Torque;

        m_Torque = Vector3.zero;
    }

    public void updateAcceleration()
    {
        m_Acceleration = m_Force * m_InverseMass;
        m_Force = Vector3.zero;
    }

    public void AddForceAtPointLocal(Vector3 force, Vector3 point)
    {
        Vector3 momentArm = (point - m_CenterOfMassWorld);
        m_Torque += Vector3.Cross(momentArm, force);
        Debug.DrawRay(point, -force, Color.blue);
        AddForce(force);
    }

    public void AddForce(Vector3 force)
    {
        m_Force += force;
    }

    public void AddForceLocal(Vector3 force)
    {
        m_Force += m_ObjectToWorldTransform.MultiplyPoint(force);
    }

    public void AddForceAtPoint(Vector3 force, Vector3 point)
    {
        Vector4 pointInWorld = m_WorldToObjectTransform * point;
        AddForceAtPointLocal(m_WorldToObjectTransform * force, new Vector3(pointInWorld.x, pointInWorld.y, pointInWorld.z) + m_Position);
    }

    public virtual void FixedUpdate()
    {
        m_ObjectToWorldTransform = Matrix4x4.TRS(m_Position, m_Rotation, Vector3.one);
        m_WorldToObjectTransform = m_ObjectToWorldTransform.transpose;

        m_InvInertiaWorldSpace = m_ObjectToWorldTransform * m_InvInertiaLocalSpace * m_WorldToObjectTransform;

        Vector4 centerRelativeWorld = m_ObjectToWorldTransform.MultiplyPoint(m_CenterOfMass);
        m_CenterOfMassWorld = new Vector3(centerRelativeWorld.x, centerRelativeWorld.y, centerRelativeWorld.z);
        updateAcceleration();
        updateAngularAcceleration();
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

        transform.position = m_Position;
        transform.rotation = m_Rotation;
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
