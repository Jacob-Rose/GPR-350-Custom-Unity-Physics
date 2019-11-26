using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CPhysicsWorld3D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhysicsNativePlugin.CreatePhysicsWorld();
        PhysicsNativePlugin.AddParticle("particle1", 0.5f);
        PhysicsNativePlugin.AddForceXToParticle("particle1", 50f);
    }

    private void OnDestroy()
    {
        PhysicsNativePlugin.DestroyPhysicsWorld();
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsNativePlugin.UpdatePhysicsWorld(Time.deltaTime);
        Vector3 pos = Vector3.zero;
        pos.x = PhysicsNativePlugin.GetParticlePosX("particle1");
        pos.y = PhysicsNativePlugin.GetParticlePosY("particle1");
        pos.z = PhysicsNativePlugin.GetParticlePosZ("particle1");
    }
}
