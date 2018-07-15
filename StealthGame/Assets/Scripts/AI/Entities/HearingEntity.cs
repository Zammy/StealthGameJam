using System.Collections.Generic;
using UnityEngine;

public class HearingEntity : IEntity
{
    public float NoiseDistanceDiminution { get; private set; }
    public List<IEntityContainer> NoiseEntities { get; private set; }

    public HearingEntity(float noiseDistanceDiminution)
    {
        NoiseDistanceDiminution = noiseDistanceDiminution;
        NoiseEntities = new List<IEntityContainer>();
    }
}