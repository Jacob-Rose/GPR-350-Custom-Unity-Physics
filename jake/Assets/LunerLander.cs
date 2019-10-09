using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunerLander : OBBHull2D
{
    public float torquePower = 2.0f;
    public float accelerationPower = 2.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A)) //rotate right
        {
            angularAcceleration += torquePower; //angularAcceleration already uses mass and time in calculation
        }
        if(Input.GetKey(KeyCode.D))
        {
            angularAcceleration -= torquePower;
        }
        if(Input.GetKey(KeyCode.W))
        {
            addForce(transform.up * accelerationPower);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            addForce(transform.up * -accelerationPower);
        }
        addForce(new Vector2(0.0f, -1.0f * Mass));
    }
}
