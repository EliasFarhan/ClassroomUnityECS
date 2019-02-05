using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private const float gravityConst = 1.0f;
    private const float centerMass = 1000.0f;

    private Vector3 velocity;

    public float planetMass;

    // Start is called before the first frame update
    void Start()
    {
        velocity = CalculateInitSpeed(transform.position, planetMass);
    }
    static Vector3 CalculateInitSpeed(Vector3 position, float planetMass) 
    {
        var deltaToCenter = - position;
        var velDir = new Vector3(-deltaToCenter.z, 0.0f, deltaToCenter.x);
        velDir.Normalize();
        var newForce = CalculateNewForce(position, planetMass);
        var speed = Mathf.Sqrt(newForce.magnitude / planetMass * 
                              deltaToCenter.magnitude);
        speed *= UnityEngine.Random.Range(0.8f, 1.2f);
        return new Vector3(velDir.x*speed, 0.0f,velDir.z*speed);
    }
    public static Vector3 CalculateNewForce(Vector3 position, float planetMass) 
    {
        var deltaToCenter = -position;
        var r = deltaToCenter.magnitude;
        var force = gravityConst * centerMass * planetMass / (r*r);
        return deltaToCenter / r * force;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var position = transform.position;
        velocity = velocity + CalculateNewForce(position, planetMass) / planetMass * Time.fixedDeltaTime;
        transform.position = velocity * Time.fixedDeltaTime + position;
    }
}
