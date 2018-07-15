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
        _player.GetEntity<HearingEntity>()
            .NoiseLocations.Clear();
        foreach (var enemy in _enemies)
        {
            var enemyHearing = enemy.GetEntity<HearingEntity>();
            enemyHearing.NoiseLocations.Clear();

            CheckHearing(enemy, _player);
            CheckHearing(_player, enemy);
        }
    }

    void CheckHearing(IEntityContainer listeningOne, IEntityContainer noiseMakingOne)
    {
        var hearing = listeningOne.GetEntity<HearingEntity>();
        var hearingPhysical = listeningOne.GetEntity<IPhysicalEntity>();
        var noiseMakingPhysical = noiseMakingOne.GetEntity<IPhysicalEntity>();
        var noise = noiseMakingOne.GetEntity<NoiseProducerEntity>();

        var diff = hearingPhysical.Position - noiseMakingPhysical.Position;
        float distance = diff.magnitude;
        float diminuation = distance * hearing.NoiseDistanceDiminution;
        if (noise.NoiseLevel - diminuation > 0)
        {
            hearing.NoiseLocations.Add(noiseMakingPhysical.Position);
        }
    }
}