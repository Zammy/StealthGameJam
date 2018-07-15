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
    readonly VisibilitySettings _settings;

    public VisibilitySystem()
    {
        _enemies = new List<IEntityContainer>();
        _settings = Resources.LoadAll<VisibilitySettings>("")[0];
    }

    public void Update()
    {
        var playerEyesight = _player.GetEntity<EyesightEntity>();
        var playerSpottable = _player.GetEntity<SpottableEntity>();
        var playerPhysical = _player.GetEntity<IPhysicalEntity>();
        foreach (var enemy in _enemies)
        {
            var enemySpottable = enemy.GetEntity<SpottableEntity>();

            enemySpottable.Spotted = IsSpotted(playerEyesight, enemySpottable);

            var enemyEyesight = enemy.GetEntity<EnemyEyesightEntity>();
            if (IsSpotted(enemyEyesight, playerSpottable))
            {
                enemyEyesight.PlayerTimeBeingSpotted += Time.deltaTime;
                if (enemyEyesight.PlayerTimeBeingSpotted > _settings.TimeForZombieToNoticePlayer)
                {
                    enemyEyesight.PlayerSpottedPosition = playerPhysical.Position;
                    enemyEyesight.FakeSpotTimeAfterVisibilityLost = _settings.FakeVisionTime;
                }
            }
            else if (enemyEyesight.FakeSpotTimeAfterVisibilityLost > 0)
            {
                enemyEyesight.FakeSpotTimeAfterVisibilityLost -= Time.deltaTime;
                enemyEyesight.PlayerSpottedPosition = playerPhysical.Position;
                enemyEyesight.PlayerTimeBeingSpotted = _settings.TimeForZombieToNoticePlayer;
            }
            else
            {
                enemyEyesight.PlayerTimeBeingSpotted = 0;
            }
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

    bool IsSpotted(EyesightEntity eyesight, SpottableEntity spottable)
    {
        Vector3 eyePos = eyesight.HeadPosition;
        var spotPos = spottable.Collider.transform.position;
        spotPos.y = _settings.TargetHeightEyesight;
        var diff = spotPos - eyePos;
        float angle = Vector3.Angle(eyesight.EyeDirection, diff);
        if (angle * 2 < eyesight.FOV)
        {
            Debug.DrawRay(eyePos, diff.normalized * eyesight.Distance, Color.red, .1f);

            var results = Physics.RaycastAll(eyePos, diff.normalized, eyesight.Distance);
            var sortedResults = results.OrderBy(k => (eyePos - k.collider.transform.position).sqrMagnitude);
            foreach (RaycastHit result in sortedResults)
            {
                if (result.collider.tag.Equals("VisibilityObstacle"))
                {
                    break;
                }
                if (result.collider == spottable.Collider)
                {
                    return true;
                }
            }
        }
        return false;
    }
}