using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    public float startMass;
    public float dampening = 0.8f;
    public Vector2 position;
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;
    public Vector2 velocity;
    public Vector2 acceleration;
    public PhysicsType calculationType;

    private Vector2 force;

    public int physicsIterations = 2;

    public float Mass
    {
        set { invMass = value > 0.0f ? 1.0f /value: 0.0f; }
        get { return 1 / invMass; }
    }
    private float invMass;
    private float startTime;
    public enum PhysicsType
    {
        Kinematic,
        Euler
    }

    void Start()
    {
        startTime = Time.time;
        Mass = startMass;
    }

    // Start is called before the first frame update

    void updatePositionEulerExplicit(float deltaTime)
    {
        position += velocity * deltaTime;
        velocity += acceleration * deltaTime;
    }

    void updatePositionKinematic(float deltaTime)
    {
        position = position + (velocity * deltaTime) + (0.5f * acceleration * deltaTime * deltaTime);
        velocity += acceleration * deltaTime;
    }

    void updateRotationEulerExplicit(float deltaTime)
    {
        rotation += angularVelocity * deltaTime;
        angularVelocity += angularAcceleration * deltaTime;
    }

    void updateRotationKinematic(float deltaTime)
    {
        rotation = rotation + (angularVelocity * deltaTime) + (0.5f * angularAcceleration * deltaTime * deltaTime);
        angularVelocity += angularAcceleration * deltaTime;
    }

    public void addForce(Vector2 newForce)
    {
        force += newForce;
    }

    public Vector2 getVelocity()
    {
        return velocity;
    }

    private void calculateAcceleration()
    {
        acceleration = force * invMass * dampening;
        force.Set(0.0f, 0.0f);
    }
    protected void FixedUpdate()
    {
        //calculation type
        float fractionTime = Time.fixedDeltaTime / physicsIterations;
        for (int i = 0; i < physicsIterations; i++)
        {
            if (calculationType == PhysicsType.Kinematic)
            {
                updatePositionKinematic(fractionTime);
                updateRotationKinematic(fractionTime);
            }
            else
            {
                updatePositionEulerExplicit(fractionTime);
                updateRotationEulerExplicit(fractionTime);
            }
        }
        //acceleration stuff
        calculateAcceleration();
        transform.position = position;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
