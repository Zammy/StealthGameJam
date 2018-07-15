using System.Collections.Generic;
using UnityEngine;

public class HearingEntity : IEntity
{
    public float NoiseDistanceDiminution { get; private set; }
    public List<Vector3> NoiseLocations { get; private set; }

    public HearingEntity(float noiseDistanceDiminution)
    {
        NoiseDistanceDiminution = noiseDistanceDiminution;
        NoiseLocations = new List<Vector3>();
    }
}