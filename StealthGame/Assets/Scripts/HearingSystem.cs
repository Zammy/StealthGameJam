using System.Collections.Generic;
using UnityEngine;

public interface IHearingSystem : IService, ITickable
{
    void AddEnemy(IEntityContainer entityContainer);
    void AddPlayer(IEntityContainer entityContainer);
}

public class HearingSystem : IHearingSystem
{
    IEntityContainer _player;
    readonly List<IEntityContainer> _enemies;
    readonly HearingSettings _settings;

    public HearingSystem()
    {
        _enemies = new List<IEntityContainer>();
        _settings = Resources.LoadAll<HearingSettings>("")[0];
    }

    public void AddEnemy(IEntityContainer entityContainer)
    {
        _enemies.Add(entityContainer);
    }

    public void AddPlayer(IEntityContainer entityContainer)
    {
        _player = entityContainer;
    }

    public void Update()
    {
        var playerNoise = _player.GetEntity<NoiseProducerEntity>();
        var playerPhysical = _player.GetEntity<PhysicalEntity>();
        foreach (var enemy in _enemies)
        {
            var enemyHearing = enemy.GetEntity<HearingEntity>();
            var enemyPhysical = enemy.GetEntity<PhysicalEntity>();

            var diff = enemyPhysical.Position - playerPhysical.Position;
            float distance = diff.magnitude;
            float diminuation = distance * enemyHearing.NoiseDistanceDiminution;
            if (playerNoise.NoiseLevel - diminuation > 0)
            {
                enemyHearing.NoiseLocation = playerPhysical.Position;
            }
            else
            {
                enemyHearing.NoiseLocation = null;
            }
        }
    }
}