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
    }

    private void OnDestroy()
    {
        PhysicsNativePlugin.DestroyPhysicsWorld();
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsNativePlugin.UpdatePhysicsWorld(Time.deltaTime);
        float[] pos = new float[3];
        IntPtr posPtr = PhysicsNativePlugin.GetParticlePos("particle1");
        IntPtr start = posPtr;
        Marshal.Copy(start, pos, 0, 3);
        Debug.Log(pos);
    }
}
