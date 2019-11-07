using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld2D : MonoBehaviour
{
    public static PhysicsWorld2D instance;

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
    
    void FixedUpdate()
    {

        for(int i =0; i < mPhysicsObjects.Count; i++)
        {
            for(int j = i+1; j < mPhysicsObjects.Count; j++)
            {
                //bool coll = mPhysicsObjects[i].detectCollision(mPhysicsObjects[j]);
                HullCollision2D collision = mPhysicsObjects[i].detectCollisionResponse(mPhysicsObjects[j]);
                if (collision != null)
                {
                    
                    collision.a.OnCollision(collision.b); //info is still valid before resolved called
                    collision.b.OnCollision(collision.a);
                    collision.Resolve(Time.fixedDeltaTime);
                }
            }
        }
    }

    public void addObject(CollisionHull2D obj)
    {
        mPhysicsObjects.Add(obj);
    }
}
