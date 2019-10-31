using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedOBBHull : OBBHull2D
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        addForce(ForceGenerator2D.GenerateForce_Gravity(Mass, -1.8f, Vector2.up));
        base.FixedUpdate();
    }
}
