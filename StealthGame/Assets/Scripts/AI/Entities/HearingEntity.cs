using UnityEngine;

public class HearingEntity : IEntity
{
    public float NoiseDistanceDiminution { get; private set; }
    public Vector3? NoiseLocation { get; set; }

    public HearingEntity(float noiseDistanceDiminution)
    {
        NoiseDistanceDiminution = noiseDistanceDiminution;
    }
}