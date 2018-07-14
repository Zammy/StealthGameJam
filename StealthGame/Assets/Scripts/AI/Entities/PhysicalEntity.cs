using UnityEngine;

public interface IPhysicalEntity : IEntity
{
    Vector3 Position { get; set; }
}

public class PhysicalEntity : IPhysicalEntity
{
    readonly Transform _transform;

    public PhysicalEntity(Transform transform)
    {
        _transform = transform;
    }

    public Vector3 Position { get { return _transform.position; } set { _transform.position = value; } }
}