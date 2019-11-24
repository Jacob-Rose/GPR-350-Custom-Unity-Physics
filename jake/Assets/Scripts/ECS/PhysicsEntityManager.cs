using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class PhysicsEntityManager : JobComponentSystem
{
    /*
    private struct Interpolate : IJobForEach<AngularComponentData>
    {
        public void Execute(ref AngularComponentData c0)
        {
            throw new System.NotImplementedException();
        }
    }
    */
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //var job = new RotationSpeedRotation() { dt = Time.deltaTime };
        //return job.Schedule(this, inputDeps);
        return new JobHandle();
    }
}
