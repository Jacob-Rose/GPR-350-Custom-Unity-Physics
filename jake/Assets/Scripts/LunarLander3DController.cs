using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarLander3DController : OBBHull3D
{
    public Transform m_ThrusterCenter;
    public float m_ThrusterDistFromCenter;

    public float m_Fuel;
    private float m_StartFuel;
    public float m_FuelPerSecond;

    public float m_ThrusterForcePerSecond;

    public override void Start()
    {
        base.Start();
        m_StartFuel = m_Fuel;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        AddForce(ForceGenerator3D.GenerateForce_Gravity(Mass, -9.8f, Vector3.up) * deltaTime);
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(-m_ThrusterDistFromCenter, 0));
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(m_ThrusterDistFromCenter, 0));
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(0, 0, m_ThrusterDistFromCenter));
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(0, 0, -m_ThrusterDistFromCenter));
        }
        
        base.FixedUpdate();
    }


    void AddThrust(float deltaTime, Vector3 offset)
    {
        if(m_Fuel > m_FuelPerSecond * deltaTime)
        {
            AddForceAtPointLocal(new Vector3(0, m_ThrusterForcePerSecond * deltaTime, 0), offset);
            m_Fuel -= m_FuelPerSecond * deltaTime;
        }
        
    }

    private void OnGUI()
    {
        Rect fuelBar = new Rect(Screen.width * 0.2f, Screen.height * 0.75f, Screen.width * 0.6f, Screen.height * 0.1f);
        GUI.BeginGroup(fuelBar);

        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(0, 0, fuelBar.width, fuelBar.height), Texture2D.whiteTexture);
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(0,0, fuelBar.width * (m_Fuel / m_StartFuel), fuelBar.height),Texture2D.whiteTexture);


        GUI.EndGroup();
    }
}
