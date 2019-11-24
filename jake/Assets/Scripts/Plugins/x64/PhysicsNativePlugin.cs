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
    public static extern void AddParticle(string id, float invMass);

    [DllImport("PhysicsDLL")]
    public static extern void RemoveParticle(string id);
    [DllImport("PhysicsDLL")]
    public static extern void SetParticlePos(string id, float[] pos);
    [DllImport("PhysicsDLL")]
    public static extern IntPtr GetParticlePos(string id);
}
