using System;
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

    public float m_Gravity = -9.8f;
    public float m_MaxSpeedOnLand = 10.0f;
    public float m_MaxAngleOnLand = 0.5f;

    public bool m_UpsidedownSinceContact = false;

    public HullCollision3D m_LastContact = null;
    public override void Start()
    {
        base.Start();
        m_StartFuel = m_Fuel;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        AddForce(ForceGenerator3D.GenerateForce_Gravity(Mass, m_Gravity, Vector3.up));
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(-m_ThrusterDistFromCenter, 0));
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(m_ThrusterDistFromCenter, 0));
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(0, 0, m_ThrusterDistFromCenter));
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.E))
        {
            AddThrust(deltaTime, new Vector3(0, 0, -m_ThrusterDistFromCenter));
        }
        if (m_LastContact != null)
        {
            HandleJustCollided(deltaTime);
            m_LastContact = null;
        }
        
        if (Vector3.Dot(transform.up, Vector3.up) < 0 && m_LastContact == null )
        {
            m_UpsidedownSinceContact = true;
        }
        
        base.FixedUpdate();
    }

    public override void OnCollision(HullCollision3D coll)
    {
        float angle = Mathf.Acos(Vector3.Dot(transform.up, Vector3.up));
        if (angle < m_MaxAngleOnLand && m_Velocity.magnitude < m_MaxSpeedOnLand)
        {
            if (LunarLanderGameManager.m_Instance != null && m_UpsidedownSinceContact)
            {
                LunarLanderGameManager.m_Instance.m_Score++;
            }
        }
        else
        {

            LunarLanderGameManager.m_Instance.ResetPlayer();
        }
        m_Fuel = m_StartFuel;
        m_UpsidedownSinceContact = false;
    }

    private void HandleJustCollided(float deltaTime)
    {
        Vector3 torqueChange = Quaternion.Lerp(m_Rotation, Quaternion.identity, 0.1f).eulerAngles;
        torqueChange.y = 0;
        for(int i = 0; i < 3; i++)
        {
            if(torqueChange[i] > 180.0f)
            {
                torqueChange[i] = torqueChange[i] - 360.0f;
            }
        }
        if(Vector3.Dot(m_AngularVelocity, torqueChange) > 0.0f)
        {
            m_Torque -= m_AngularVelocity* 0.4f * Mass;
        }
        m_Torque -= torqueChange * deltaTime;
        m_AngularVelocity *= 1.0f - deltaTime;
        AddForce(ForceGenerator3D.GenerateForce_Friction_Kinetic(Vector3.up, m_Velocity, 0.7f) * Mass * -m_Gravity);
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
        GUI.color = Color.black;
        GUIStyle textStyle = GUI.skin.label;
        textStyle.alignment = TextAnchor.MiddleCenter;
        if(m_UpsidedownSinceContact)
        {
            GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.2f), "NOW LAND!", textStyle);
        }


        Rect fuelBar = new Rect(Screen.width * 0.2f, Screen.height * 0.75f, Screen.width * 0.6f, Screen.height * 0.1f);
        GUI.BeginGroup(fuelBar);
        GUI.DrawTexture(new Rect(0, 0, fuelBar.width, fuelBar.height), Texture2D.whiteTexture);
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(0,0, fuelBar.width * (m_Fuel / m_StartFuel), fuelBar.height),Texture2D.whiteTexture);
        GUI.EndGroup();
    }
}
