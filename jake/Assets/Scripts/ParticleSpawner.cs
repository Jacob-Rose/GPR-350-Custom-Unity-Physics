using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public Particle2D reference;
    public Particle2D.PhysicsType type;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnParticle", 0.0f, 1.0f);
    }

    void SpawnParticle()
    {
       
    }
}
