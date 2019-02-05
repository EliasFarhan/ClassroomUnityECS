
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private Planet planetPrefab;

    [SerializeField] private float maxRadius = 100.0f;

    [SerializeField] private int planetCount = 100;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < planetCount; i++)
        {
            Vector3 position = new Vector3(
                UnityEngine.Random.Range(-maxRadius, maxRadius), 
                0.0f, 
                UnityEngine.Random.Range(-maxRadius, maxRadius));
            if (position.magnitude > maxRadius)
            {
                position = position.normalized * maxRadius;
            }
            var planet = Instantiate(planetPrefab, 
                position, 
                Quaternion.identity);
            planet.planetMass = Random.Range(0.8f, 1.2f);
        }
    }

}
