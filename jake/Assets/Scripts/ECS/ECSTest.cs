using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ECSTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EntityManager manager = World.Active.EntityManager;
        Entity entity = manager.CreateEntity(typeof(ParticleComponentData));
        manager.SetComponentData(entity, new ParticleComponentData { m_Velocity = new Vector3(1.0f,0.0f,0.0f)});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
