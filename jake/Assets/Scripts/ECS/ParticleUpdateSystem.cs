using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ParticleUpdateSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref ParticleComponentData particleComp) => {
            particleComp.Update();
            Debug.Log(particleComp.m_Position);
        });
    }
}
