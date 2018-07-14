using UnityEngine;

public class SpottableEntity : IEntity
{
    public Collider Collider { get; private set; }

    public virtual bool Spotted { get; set; }

    public SpottableEntity(Collider collider)
    {
        Collider = collider;
    }
}