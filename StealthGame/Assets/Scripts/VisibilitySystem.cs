using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IVisibilitySystem : IService, ITickable
{
    void AddEnemy(IEntityContainer entityContainer);
    void AddPlayer(IEntityContainer entityContainer);
}

public class VisibilitySystem : IVisibilitySystem
{
    IEntityContainer _player;
    readonly List<IEntityContainer> _enemies;

    public VisibilitySystem()
    {
        _enemies = new List<IEntityContainer>();
    }

    public void Update()
    {
        var eyesight = _player.GetEntity<EyesightEntity>();
        var playerPhysical = _player.GetEntity<IPhysicalEntity>();
        foreach (var e in _enemies)
        {
            var enemyPhysical = e.GetEntity<IPhysicalEntity>();
            var enemySpottable = e.GetEntity<SpottableEntity>();
            bool spotted = false;

            var playerToEnemyDirection = enemyPhysical.Position - playerPhysical.Position;

            float angle = Vector3.Angle(eyesight.EyeDirection, playerToEnemyDirection);
            if (angle * 2 < eyesight.FOV)
            {
                playerToEnemyDirection.y = .5f;
                var results = Physics.RaycastAll(eyesight.HeadPosition, playerToEnemyDirection, eyesight.Distance);
                var sortedResults = results.OrderBy(k => (playerPhysical.Position - k.collider.transform.position).sqrMagnitude);
                foreach (RaycastHit result in sortedResults)
                {
                    if (result.collider.tag.Equals("VisibilityObstacle"))
                    {
                        break;
                    }
                    if (result.collider == enemySpottable.Collider)
                    {
                        spotted = true;
                        break;
                    }
                }
            }
            enemySpottable.Spotted = spotted;
        }
    }

    public void AddEnemy(IEntityContainer entityContainer)
    {
        _enemies.Add(entityContainer);
    }

    public void AddPlayer(IEntityContainer entityContainer)
    {
        _player = entityContainer;
    }
}