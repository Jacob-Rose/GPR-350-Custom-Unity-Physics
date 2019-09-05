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
    public PathType pathType;
    public int physicsIterations = 2;
    private float startTime; //used to randomize(not all objects in sync)
    public enum PathType
    {
        Circle,
        LineSin,
        None
    }
    public enum PhysicsType
    {
        Kinematic,
        Euler
    }

    void Start()
    {
        startTime = Time.time;
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

    void FixedUpdate()
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
        if(pathType == PathType.Circle)
        {
            //use velocity to ensure proper circle
            acceleration.x = Mathf.Sin(Time.fixedTime - startTime);
            acceleration.y = Mathf.Cos(Time.fixedTime - startTime);
        }
        else if(pathType == PathType.LineSin)
        {
            //same as circle but only on X
            acceleration.x = Mathf.Sin(Time.fixedTime - startTime);
        }
        transform.position = position;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
