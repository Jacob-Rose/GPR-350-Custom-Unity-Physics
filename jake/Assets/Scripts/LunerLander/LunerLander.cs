using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LunerLander : OBBHull2D
{
    private Vector2 startVelocity;
    private Vector2 startPosition;
    private float startRotation;
    public Sprite[] sprites;
    public float fuel = 100.0f;
    public float fuelConsumptionPerAcceleration = 100.0f;
    public float rotationCorrectionForce = 2.0f;

    public float maxThrust = 10.0f;
    public float timeToMaxThrust = 2.0f;

    public float maxVelocityMagnitudeOnLanding = 4.0f;
    public float maxVelocityMagnitudeOnSafeLanding = 2.0f;
    public float maxRotationOffsetOnLanding = 4.0f;

    public float currentThrust = 0.0f;


    public override void Start()
    {
        base.Start();
        startPosition = position;
        startVelocity = velocity;
        startRotation = rotation;
    }
    public float torquePower = 2.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if(!LunerGameManager.Instance.HasGameStarted)
        {
            return;
        }
        if(!LunerGameManager.Instance.IsTransitioning || LunerGameManager.Instance.HasGameEnded)
        {
            if (!LunerGameManager.Instance.HasGameEnded)
            {
                HandleInput(Time.fixedDeltaTime);
            }
            addForce(ForceGenerator2D.GenerateForce_Gravity(Mass, -0.8f, Vector2.up));
            base.FixedUpdate();
            CheckRotation(Time.fixedDeltaTime);
            HandleUpdate(Time.fixedDeltaTime);
            
        }
        
        

        if (fuel <= 0.0f)
        {
            LunerGameManager.Instance.EndGame();
        }
    }

    public void CheckRotation(float deltaTime)
    {
        if (rotation + (angularVelocity* deltaTime) > 90.0f || rotation + (angularVelocity* deltaTime) < -90.0f)
        {
            angularVelocity = 0.0f;
            rotation = Mathf.Clamp(rotation + (angularVelocity*deltaTime), -90.0f, 90.0f);
        }
    }


    public void HandleInput(float deltaTime)
    {
        if (Input.GetKey(KeyCode.A)) //rotate right
        {
            angularAcceleration += torquePower; //angularAcceleration already uses mass and time in calculation
        }
        if (Input.GetKey(KeyCode.D))
        {
            angularAcceleration -= torquePower;
        }
        if (Input.GetKey(KeyCode.W))
        {
            currentThrust += (maxThrust / timeToMaxThrust) * deltaTime;
            addForce(transform.up * currentThrust);
            fuel -= fuelConsumptionPerAcceleration * deltaTime;
        }
        else
        {
            currentThrust -= (maxThrust / timeToMaxThrust) * deltaTime * 2;
        }
        currentThrust = Mathf.Clamp(currentThrust, 0.0f, maxThrust);
        
    }
    public void HandleUpdate(float deltaTime)
    {
        
        if(currentThrust > maxThrust * 0.5f)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[2];
        }
        else if(currentThrust > 0.0f)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[1];
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
        }

        //var emmision = GetComponentInChildren<ParticleSystem>().emission;
        //emmision.rateOverTime = new ParticleSystem.MinMaxCurve(40.0f * (currentThrust / maxThrust));
        
    }

    public override void OnCollision(CollisionHull2D withObject)
    {
        if(LunerGameManager.Instance.HasGameEnded)
        {
            return;
        }
        if(velocity.magnitude > maxVelocityMagnitudeOnLanding || rotation < -maxRotationOffsetOnLanding || rotation > maxRotationOffsetOnLanding)
        {
            LunerGameManager.Instance.ShowTransitionMessage("Game Over");
            LunerGameManager.Instance.EndGame();
        }
        else if(withObject is LunerPlatform)
        {
            if(velocity.magnitude > maxVelocityMagnitudeOnSafeLanding)
            {
                LunerPlatform collidedWith = withObject as LunerPlatform;
                LunerGameManager.Instance.addToScore(collidedWith.score);
                ResetAndShowMessage("Shakey landing, but there in one piece");
            }
            else
            {
                LunerPlatform collidedWith = withObject as LunerPlatform;
                LunerGameManager.Instance.addToScore(collidedWith.score * 1.5f);
                fuel += collidedWith.fuelGained;
                ResetAndShowMessage("Clean Landing, gained fuel and bonus points");
            }
            
        }
        //all collisions happen below, so if collision occured then lerp the rotation
        addForce(ForceGenerator2D.GenerateForce_Friction_Kinetic(withObject.transform.up, velocity, 2.0f));
        if (rotation > 5.0f)
        {
            torque = -rotationCorrectionForce * (rotation/360);
        }
        else if (rotation < -5.0f)
        {
            torque = rotationCorrectionForce * (-rotation / 360);
        }
        else 
        { 
            torque = -rotation * rotationCorrectionForce * Time.deltaTime;
        }
    }

    public void ResetAndShowMessage(string message)
    {
        LunerGameManager.Instance.ShowTransitionMessage(message);
        ResetGame();
    }

    public void LoseGame()
    {
        //TODO

    }

    public void ResetGame()
    {
        position = startPosition;
        velocity = startVelocity;
        rotation = startRotation;
    }
}
