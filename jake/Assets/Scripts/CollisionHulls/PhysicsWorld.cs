using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld : MonoBehaviour
{
    public static PhysicsWorld instance;

    public List<CollisionHull2D> mPhysicsObjects = new List<CollisionHull2D>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i < mPhysicsObjects.Count; i++)
        {
            for(int j = 0; j < mPhysicsObjects.Count; j++)
            {
                bool coll = mPhysicsObjects[i].detectCollision(mPhysicsObjects[j]);
                if (i != j && coll)
                {
                    Debug.Log("Collision Happened");
                }
            }
        }
    }

    public void addObject(CollisionHull2D obj)
    {
        mPhysicsObjects.Add(obj);
    }
}
