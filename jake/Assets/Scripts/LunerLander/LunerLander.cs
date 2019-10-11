using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LunerLander : OBBHull2D
{
    public Sprite[] sprites;
    public float fuel = 100.0f;
    public float fuelConsumptionPerAcceleration = 100.0f;

    public float maxThrust = 10.0f;
    public float timeToMaxThrust = 2.0f;

    public float currentThrust = 0.0f;

    public TextMeshProUGUI fuelText;

    public float torquePower = 2.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    public override void FixedUpdate()
    {
        HandleUpdate(Time.fixedDeltaTime);
        base.FixedUpdate();
    }

    public void HandleUpdate(float deltaTime)
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
        if(currentThrust > 0.0f)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[1];
        }
        else if(currentThrust > maxThrust * 0.5f)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[2];
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
        }
        addForce(ForceGenerator2D.GenerateForce_Gravity(Mass, -0.8f, Vector2.up));

        fuelText.text = "Fuel Left: " + fuel.ToString("F1");
    }

    public override void OnCollision(CollisionHull2D withObject)
    {
        base.OnCollision(withObject);
    }
}
