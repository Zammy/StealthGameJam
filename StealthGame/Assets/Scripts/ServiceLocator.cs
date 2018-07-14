using System;
using System.Collections.Generic;
using System.Linq;

public interface IService
{
}

public interface IInitializable
{
    void Init();
}

public interface ILateInitializable
{
    void LateInit();
}

public interface IDestroyable
{
    void Destroy();
}

public class ServiceLocator : Singleton<ServiceLocator>
{
    List<IService> _services = new List<IService>();

    public void RegisterService(IService service)
    {
        if (!_services.Contains(service))
        {
            _services.Add(service);
        }
    }

    public void UnregisterService(IService service)
    {
        _services.Remove(service);
    }

    public T GetService<T>()
    {
        for (int i = 0; i < _services.Count; i++)
        {
            if (typeof(T).IsAssignableFrom(_services[i].GetType()))
            {
                return (T)_services[i];
            }
        }

        return default(T);
    }

    public IService GetService(Type type)
    {
        for (int i = 0; i < _services.Count; i++)
        {
            if (type.IsAssignableFrom(_services[i].GetType()))
            {
                return _services[i];
            }
        }
        return null;
    }

    public void InitServices()
    {
        var toInit = GetServicesThatImplement<IInitializable>();
        foreach (var item in toInit)
        {
            item.Init();
        }
    }

    public void LateInitServices()
    {
        var toInit = GetServicesThatImplement<ILateInitializable>();
        foreach (var item in toInit)
        {
            item.LateInit();
        }
    }

    public void DestroyServices()
    {
        var toDestroy = GetServicesThatImplement<IDestroyable>();
        foreach (var item in toDestroy)
        {
            item.Destroy();
        }
    }

    public IEnumerable<T> GetServicesThatImplement<T>()
    {
        return _services.Where(service => typeof(T).IsAssignableFrom(service.GetType()))
                .Cast<T>();
    }
}