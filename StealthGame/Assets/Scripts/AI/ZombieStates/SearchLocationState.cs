using System.Collections;
using UnityEngine;

public class SearchLocationState : IOverrideState
{
    readonly IEntityContainer _entityContainer;

    SearchLocationStateData _data;

    public SearchLocationState(IEntityContainer entityContainer)
    {
        _entityContainer = entityContainer;
    }

    public void SetData(SMStateData data)
    {
        _data = (SearchLocationStateData)data;
    }

    public void OnStateEnter()
    {
        var agent = _entityContainer.GetEntity<INavMeshAgent>();
        agent.MovementSpeed = _data.MovementSpeed;
    }

    public bool OverrideCurrentState(ISMState currentState)
    {
        var hearing = _entityContainer.GetEntity<HearingEntity>();
        return hearing.NoiseLocation.HasValue && currentState.GetType() != typeof(ChaseState);
    }

    public IEnumerator OnStateExecute(StateMachine sm)
    {
        var hearing = _entityContainer.GetEntity<HearingEntity>();
        yield return sm.StartCoroutine(Move.Do(_entityContainer, hearing.NoiseLocation.Value));

        var phyiscal = _entityContainer.GetEntity<IPhysicalEntity>();
        yield return sm.StartCoroutine(Rotation.Do(phyiscal, _data.AngleLookAround, _data.LookAroundDuration / 4));
        yield return sm.StartCoroutine(Rotation.Do(phyiscal, _data.AngleLookAround * -2, _data.LookAroundDuration / 2));
        yield return sm.StartCoroutine(Rotation.Do(phyiscal, _data.AngleLookAround, _data.LookAroundDuration / 4));

        sm.ChangeState(typeof(WanderingState));
    }

    public void OnStateExit()
    {
    }


}