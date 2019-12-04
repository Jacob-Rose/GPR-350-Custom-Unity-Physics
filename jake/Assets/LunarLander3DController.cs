using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionHull3D))]
public class LunarLander3DController : MonoBehaviour
{
    public Transform m_ThrusterCenter;
    public float m_ThrusterDistFromCenter;

    public float m_ThrusterForcePerSecond;

    private CollisionHull3D m_CollisionHull;
    void Start()
    {
        m_CollisionHull = GetComponent<CollisionHull3D>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        if(Input.GetKey(KeyCode.A))
        {
            AddThrust(deltaTime, new Vector3(0, m_ThrusterDistFromCenter, 0));
        }
        if(Input.GetKey(KeyCode.W))
        {
            AddThrust(deltaTime, new Vector3(0, -m_ThrusterDistFromCenter, 0));
        }
        if(Input.GetKey(KeyCode.S))
        {
            AddThrust(deltaTime, new Vector3(m_ThrusterDistFromCenter, 0, 0));
        }
        if(Input.GetKey(KeyCode.D))
        {
            AddThrust(deltaTime, new Vector3(-m_ThrusterDistFromCenter, 0, 0));
        }
    }


    void AddThrust(float deltaTime, Vector3 offset)
    {
        m_CollisionHull.AddForceAtPoint(new Vector3(0, -m_ThrusterForcePerSecond * deltaTime, 0), offset);
    }
}
