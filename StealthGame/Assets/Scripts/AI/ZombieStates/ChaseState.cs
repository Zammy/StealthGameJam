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

    public bool OverrideCurrentState(ISMState currentState)
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
        var physical = _entityContainer.GetEntity<IPhysicalEntity>();
        var agent = _entityContainer.GetEntity<INavMeshAgent>();
        while (eyesight.PlayerSpottedPosition.HasValue)
        {
            var targetPos = eyesight.PlayerSpottedPosition.Value;
            agent.SetDestination(targetPos);
            yield return null;
            if ((targetPos - physical.Position).sqrMagnitude < .5f)
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