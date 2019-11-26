using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class PhysicsEntityManager : MonoBehaviour
{
    TransformAccessArray transforms;
    JobHandle integrationHandle;
    IntegrationJob integrationJob;

    void OnDisable()
    {
        integrationHandle.Complete();
        transforms.Dispose();
    }

    void Start()
    {
        transforms = new TransformAccessArray(0, -1);
    }

    void Update()
    {
        integrationHandle.Complete();
        if(Input.GetKeyDown(KeyCode.S))
        {
            AddPhysicsObject(1);
        }

        integrationJob = new IntegrationJob()
        {

        };

        integrationHandle = integrationJob.Schedule(transforms);
    }

    void AddPhysicsObject(int count)
    {
        integrationHandle.Complete();

        transforms.capacity = transforms.length + count;
        for(int i = 0; i < count; i++)
        {
            //transforms.Add()
        }
    }
}
