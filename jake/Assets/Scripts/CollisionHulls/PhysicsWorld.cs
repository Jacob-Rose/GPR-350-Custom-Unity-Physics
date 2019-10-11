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
                    collision.Resolve();
                    collision.a.OnCollision(collision.b);
                    collision.b.OnCollision(collision.a);
                    
                    Debug.Log("Collision Occured between " + mPhysicsObjects[i].gameObject.name + " " + mPhysicsObjects[j].gameObject.name);
                }
            }
        }
    }

    public void addObject(CollisionHull2D obj)
    {
        mPhysicsObjects.Add(obj);
    }
}
