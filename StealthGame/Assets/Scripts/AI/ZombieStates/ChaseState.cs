using System.Collections;
using UnityEngine;

public class ChaseState : IOverrideState
{
    readonly IEntityContainer _entityContainer;

    ChaseStateData _data;

    public ChaseState(IEntityContainer entityContainer)
    {
        _entityContainer = entityContainer;
    }

    public void SetData(SMStateData data)
    {
        _data = (ChaseStateData)data;
    }

    public bool OverrideCurrentState()
    {
        var eyesight = _entityContainer.GetEntity<EnemyEyesightEntity>();
        return eyesight.PlayerSpottedPosition != null;
    }

    public void OnStateEnter()
    {
        var agent = _entityContainer.GetEntity<INavMeshAgent>();
        agent.MovementSpeed = _data.MovementSpeed;
    }

    public IEnumerator OnStateExecute(StateMachine stateMachine)
    {
        var eyesight = _entityContainer.GetEntity<EnemyEyesightEntity>();
        while (eyesight.PlayerSpottedPosition.HasValue)
        {
            var targetPos = eyesight.PlayerSpottedPosition.Value;
            yield return Move.Do(_entityContainer, targetPos);
            if ((eyesight.PlayerSpottedPosition.Value - targetPos).sqrMagnitude < 4f)
            {
                eyesight.PlayerSpottedPosition = null;
            }
        }
        stateMachine.ChangeState(typeof(WanderingState));
    }

    public void OnStateExit()
    {
    }

}