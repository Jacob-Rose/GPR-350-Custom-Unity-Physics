using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class PlayerBall : MonoBehaviour
{
    public Vector2 posToAddForce = new Vector2(1.0f,0.0f);
    public float intensity = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        GetComponent<Particle2D>().addForceAtPoint(posToAddForce, new Vector2(x * intensity, y * intensity));
    }
}
