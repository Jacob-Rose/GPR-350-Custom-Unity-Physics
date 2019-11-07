using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWorld3D : MonoBehaviour
{
    public static PhysicsWorld3D instance;

    public List<CollisionHull3D> mPhysicsObjects = new List<CollisionHull3D>();

    private void Awake()
    {
        if (instance == null)
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
        for (int i = 0; i < mPhysicsObjects.Count; i++)
        {
            for (int j = i + 1; j < mPhysicsObjects.Count; j++)
            {
                //bool coll = mPhysicsObjects[i].detectCollision(mPhysicsObjects[j]);
                HullCollision3D collision = mPhysicsObjects[i].DetectCollision(mPhysicsObjects[j]);
                if (collision != null)
                {
                    //collision.a.OnCollision(collision.b); //info is still valid before resolved called
                    //collision.b.OnCollision(collision.a);
                    //collision.Resolve(Time.fixedDeltaTime);
                    Debug.Log("3D collision occured");
                }
            }
        }
    }

    public void addObject(CollisionHull3D obj)
    {
        mPhysicsObjects.Add(obj);
    }
}
