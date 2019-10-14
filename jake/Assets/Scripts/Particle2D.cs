using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Particle2D : MonoBehaviour
{
    public float startMass = 10.0f; 
    public float dampening = 0.8f;
    public Vector2 position;
    public float rotation;
    private float angularVelocity;
    public float angularAcceleration;
    public Vector2 velocity;
    public Vector2 acceleration;
    public PhysicsType calculationType;
    public ShapeType shapeType;
    [Tooltip("Circle = Radius, Ring = OuterRadius, Rectangle = XLength, Line = length")]
    public float inertiaVar1;
    [Tooltip("Circle = ignore, Ring = inner radius, Rectangle = YLength, line = ignore")]
    public float inertiaVar2;
    private Vector2 force;
    public bool shouldForceAtPointMove = true;

    public float restitution = 0.5f;

    public int physicsIterations = 2;
    public float Mass
    {
        set { invMass = value > 0.0f ? 1.0f / value : 0.0f; }
        get { return 1 / invMass; }
    }

    public float InverseMass
    {
        get { return invMass; }
    }

    protected float invMass;
    protected float startTime;
    protected float torque;
    protected float inertia;
    public enum PhysicsType
    {
        Kinematic,
        Euler
    }

    public enum ShapeType
    {
        Circle,
        Ring,
        Rectangle,
        Line
    }

    public virtual void Start()
    {
        startTime = Time.time;
        Mass = startMass;
        inertia = calculateInertia();
        position = transform.position + new Vector3(position.x, position.y, 0);
    }

    private float calculateInertia()
    {
        if(shapeType == ShapeType.Circle)
        {
            return 0.5f * Mass * inertiaVar1 * inertiaVar1; //var1 is radius set by user
        }
        else if(shapeType == ShapeType.Rectangle)
        {
            return 0.0833f * Mass * ((inertiaVar1 * inertiaVar1) + (inertiaVar2 * inertiaVar2));
        }
        else if(shapeType == ShapeType.Ring)
        {
            return 0.5f * Mass * ((inertiaVar1 * inertiaVar1) + (inertiaVar2 * inertiaVar2));
        }
        else if(shapeType == ShapeType.Line)
        {
            return 0.0833f * Mass * (inertiaVar1 * inertiaVar1);
        }
        throw new System.DivideByZeroException();
    }

    void updateAngularAcceleration()
    {
        if (invMass == 0.0f)
            return;
        angularAcceleration = (torque / inertia);
        torque = 0.0f;
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

    //point is relative to center
    public void addForceAtPoint(Vector2 pointRelativeToCenter, Vector2 force)
    {
        Vector3 dir = new Vector3(pointRelativeToCenter.x, pointRelativeToCenter.y, 0.0f);
        Vector3 forcePos = new Vector3(force.x, force.y, 0.0f);
        Debug.DrawLine(transform.position + dir, transform.position + dir + forcePos, Color.magenta, 0.1f);
        torque += pointRelativeToCenter.x * force.y - pointRelativeToCenter.y * force.x;
        if(shouldForceAtPointMove)
            addForce(force);
    }

    public Vector2 getVelocity()
    {
        return velocity;
    }

    public void setVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    private void calculateAcceleration()
    {
        acceleration = force * invMass * dampening;
        force.Set(0.0f, 0.0f);
    }
    public virtual void FixedUpdate()
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
        updateAngularAcceleration();
        //acceleration stuff
        calculateAcceleration();

        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
