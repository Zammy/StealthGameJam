public enum NoiseType
{
    Footsteps,
    Roar,
    ClickerScream
}

public class NoiseProducerEntity : IEntity
{
    public float NoiseLevel;
    public NoiseType Type;
}