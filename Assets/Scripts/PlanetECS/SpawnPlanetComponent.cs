using System;
using Unity.Entities;
using UnityEngine;


[Serializable]
public struct SpawnPlanetData : ISharedComponentData, IEquatable<SpawnPlanetData>
{
    public GameObject prefab;
    public float maxRadius;
    public int count;

    public bool Equals(SpawnPlanetData other)
    {
        return Equals(prefab, other.prefab) && maxRadius.Equals(other.maxRadius) && count == other.count;
    }

    public override bool Equals(object obj)
    {
        return obj is SpawnPlanetData other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (prefab != null ? prefab.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ maxRadius.GetHashCode();
            hashCode = (hashCode * 397) ^ count;
            return hashCode;
        }
    }
}

public class SpawnPlanetComponent : SharedComponentDataProxy<SpawnPlanetData> { }
