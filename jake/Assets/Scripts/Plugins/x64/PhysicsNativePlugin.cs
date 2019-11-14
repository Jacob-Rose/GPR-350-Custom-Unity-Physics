using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PhysicsNativePlugin
{
    [DllImport("PhysicsDLL")]
    public static extern void CreatePhysicsWorld();
    [DllImport("PhysicsDLL")]
    public static extern void DestroyPhysicsWorld();
    [DllImport("PhysicsDLL")]
    public static extern void UpdatePhysicsWorld(float deltaTime);
    [DllImport("PhysicsDLL")]
    public static extern void AddParticle(string id, float invMass, float[] pos);

    [DllImport("PhysicsDLL")]
    public static extern IntPtr GetPhysicsObjectPos(string id);

    public static float[] GetPos(string id)
    {
        IntPtr ptr = GetPhysicsObjectPos(id);
        float[] floats = new float[3];
        Marshal.Copy(ptr, floats, 0, floats.Length);
        return floats;
    }
}
