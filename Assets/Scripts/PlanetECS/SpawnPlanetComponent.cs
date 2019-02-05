using System;
using Unity.Entities;
using UnityEngine;


[Serializable]
public struct SpawnPlanetData : ISharedComponentData
{
    public GameObject prefab;
    public float maxRadius;
    public int count;
}

public class SpawnPlanetComponent : SharedComponentDataWrapper<SpawnPlanetData> { }
