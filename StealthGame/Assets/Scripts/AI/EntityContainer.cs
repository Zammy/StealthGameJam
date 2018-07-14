using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IEntity
{
}

public interface IEntityContainer
{
    T GetEntity<T>() where T : IEntity;
    void AddEntity(IEntity entity);
    void RemoveEntity(IEntity entity);
}

public class EntityContainer : MonoBehaviour, IEntityContainer
{
    void Awake()
    {
        _entities = new List<IEntity>();
    }

    public T GetEntity<T>() where T : IEntity
    {
        return _entities.OfType<T>().FirstOrDefault();
    }

    public void AddEntity(IEntity entity)
    {
        _entities.Add(entity);
    }

    public void RemoveEntity(IEntity entity)
    {
        _entities.Remove(entity);
    }

    List<IEntity> _entities;
}