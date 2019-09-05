using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    public Vector2 position;
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;
    public Vector2 velocity;
    public Vector2 acceleration;
    public PhysicsType calculationType;

    public enum PhysicsType
    {
        Kinematic,
        Euler
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (calculationType == PhysicsType.Kinematic)
        {
            updatePositionKinematic(Time.fixedDeltaTime);
            updateRotationKinematic(Time.fixedDeltaTime);
        }
        else
        {
            updatePositionEulerExplicit(Time.fixedDeltaTime);
            updateRotationEulerExplicit(Time.fixedDeltaTime);
        }
        
        transform.position = position;
        transform.eulerAngles = new Vector3(0, 0, rotation);
        acceleration.x = Mathf.Sin(Time.fixedTime);
    }
}
