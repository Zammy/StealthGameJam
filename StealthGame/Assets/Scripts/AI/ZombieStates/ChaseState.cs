using System.Collections;
using UnityEngine;

public class ChaseState : BaseState, IOverrideState
{
    public ChaseState(IEntityContainer entityContainer)
        : base(entityContainer)
    {
    }

    public bool ShouldOverrideCurrentState(ISMState currentState)
    {
        var eyesight = _entityContainer.GetEntity<EnemyEyesightEntity>();
        return eyesight.PlayerSpottedPosition != null;
    }

    public override IEnumerator OnStateExecute(StateMachine stateMachine)
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

}