using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PlanetData : IComponentData
{
    public float3 velocity;
    public float mass;
}

[UnityEngine.DisallowMultipleComponent]
public class PlanetDataComponent : ComponentDataWrapper<PlanetData> { }