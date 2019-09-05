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
        InvokeRepeating("SpawnParticle", 0.0f, 1.0f);
    }

    void SpawnParticle()
    {
        GameObject clone = Instantiate(reference.gameObject);
        clone.SetActive(false);
        clone.GetComponent<Particle2D>().calculationType = type;
        float rand = Random.value;
        
        if (rand > 1.0f)//random dir
        {
            clone.GetComponent<Particle2D>().pathType = Particle2D.PathType.None;
            clone.GetComponent<Particle2D>().velocity = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
            clone.GetComponent<Particle2D>().acceleration = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
        }
        else if(rand > 0.5f) //circle
        {
            //based off the calculation in Particle2D, 
            //cannot add to start of Particle2D as the pathtype is set after creation
            clone.GetComponent<Particle2D>().pathType = Particle2D.PathType.Circle;
            clone.GetComponent<Particle2D>().velocity = new Vector2(-1.0f, 0.0f); 
        }
        else //linesin
        {
            //based off the calculation in Particle2D, 
            //cannot add to start of Particle2D as the pathtype is set after creation
            clone.GetComponent<Particle2D>().pathType = Particle2D.PathType.LineSin;
            clone.GetComponent<Particle2D>().velocity = new Vector2(-1.0f, 0.0f);
        }
        clone.GetComponent<Particle2D>().position = transform.position;
        clone.GetComponent<Particle2D>().angularAcceleration = Random.value;
        clone.SetActive(true);
    }
}
