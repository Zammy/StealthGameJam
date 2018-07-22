using System.Collections;
using UnityEngine;

public class SearchLocationState : BaseState, IOverrideState
{
    public SearchLocationState(IEntityContainer entityContainer)
        : base(entityContainer)
    {
    }

    public bool ShouldOverrideCurrentState(ISMState currentState)
    {
        var hearing = _entityContainer.GetEntity<HearingEntity>();
        return hearing.NoiseEntities.Count > 0
            && currentState.GetType() != typeof(ChaseState)
            && currentState.GetType() != typeof(ClickerChaseState);
    }

    public override IEnumerator OnStateExecute(StateMachine sm)
    {
        var hearing = _entityContainer.GetEntity<HearingEntity>();
        var agent = _entityContainer.GetEntity<INavMeshAgent>();
        var noise = _entityContainer.GetEntity<NoiseProducerEntity>();
        noise.NoiseLevel += Data.WalkExtraNoise;
        while (hearing.NoiseEntities.Count > 0)
        {
            var noisePos = hearing.NoiseEntities[0].GetEntity<IPhysicalEntity>().Position;
            agent.SetDestination(noisePos);
            yield return null;
        }
        while (!agent.IsDestinationReached())
        {
            yield return null;
        }
        noise.NoiseLevel -= Data.WalkExtraNoise;

        var phyiscal = _entityContainer.GetEntity<IPhysicalEntity>();
        yield return sm.StartCoroutine(Rotation.Do(phyiscal, Data.AngleLookAround, Data.LookAroundDuration / 4));
        yield return sm.StartCoroutine(Rotation.Do(phyiscal, Data.AngleLookAround * -2, Data.LookAroundDuration / 2));
        yield return sm.StartCoroutine(Rotation.Do(phyiscal, Data.AngleLookAround, Data.LookAroundDuration / 4));

        sm.ChangeState(typeof(WanderingState));
    }

    SearchLocationStateData Data { get { return (SearchLocationStateData)base._data; } }
}