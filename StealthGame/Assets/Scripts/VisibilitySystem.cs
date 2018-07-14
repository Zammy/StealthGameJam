using System.Collections.Generic;

public interface IVisibilitySystem : IService, ITickable
{
    void AddEntity(IEntityContainer entityContainer);
}

public class VisibilitySystem : IVisibilitySystem
{
    readonly List<IEntityContainer> _entities;

    public VisibilitySystem()
    {
        _entities = new List<IEntityContainer>();
    }

    public void Update()
    {
    }

    public void AddEntity(IEntityContainer entityContainer)
    {
        _entities.Add(entityContainer);
    }
}