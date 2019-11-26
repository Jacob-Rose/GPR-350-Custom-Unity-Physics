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
    public static extern void SetParticlePosX(string id, float pos);
    [DllImport("PhysicsDLL")]
    public static extern void SetParticlePosY(string id, float pos);
    [DllImport("PhysicsDLL")]
    public static extern void SetParticlePosZ(string id, float pos);
    [DllImport("PhysicsDLL")]
    public static extern float GetParticlePosX(string id);
    [DllImport("PhysicsDLL")]
    public static extern float GetParticlePosY(string id);
    [DllImport("PhysicsDLL")]
    public static extern float GetParticlePosZ(string id);
    [DllImport("PhysicsDLL")]
    public static extern void AddForceXToParticle(string id, float force);
    [DllImport("PhysicsDLL")]
    public static extern void AddForceYToParticle(string id, float force);
    [DllImport("PhysicsDLL")]
    public static extern void AddForceZToParticle(string id, float force);
}
