using System.Collections.Generic;
using System.Linq;
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
            .NoiseEntities.Clear();
        foreach (var enemy in _enemies)
        {
            var enemyHearing = enemy.GetEntity<HearingEntity>();
            enemyHearing.NoiseEntities.Clear();

            CheckHearing(enemy, _player);
            CheckHearing(_player, enemy);

            var enemyNoise = enemy.GetEntity<NoiseProducerEntity>();
            if (enemyNoise.Type == NoiseType.ClickerScream)
            {
                foreach (var otherEnemy in _enemies.Where(e => e != enemy))
                {
                    CheckHearing(otherEnemy, enemy);
                }
            }
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
            hearing.NoiseEntities.Add(noiseMakingOne);
        }
    }
}