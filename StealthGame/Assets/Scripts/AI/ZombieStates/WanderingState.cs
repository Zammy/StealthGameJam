using System.Collections;
using UnityEngine;

public class WanderingState : BaseState
{
    public WanderingState(IEntityContainer entityContainer)
        : base(entityContainer)
    {
    }

    public override IEnumerator OnStateExecute(StateMachine stateMachine)
    {
        var physicalEntity = _entityContainer.GetEntity<IPhysicalEntity>();
        var agent = _entityContainer.GetEntity<INavMeshAgent>();
        var noise = _entityContainer.GetEntity<NoiseProducerEntity>();
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(Data.MinWait, Data.MaxWait));

            float distance = Random.Range(Data.MinDistance, Data.MaxDistance);
            Vector3 dest = physicalEntity.Position.RandomPosAround(distance);
            dest = agent.GetClosestToNavMeshPoint(dest);
            noise.NoiseLevel += Data.ExtraNoiseWhenWalking;
            yield return Move.Do(_entityContainer, dest);
            noise.NoiseLevel -= Data.ExtraNoiseWhenWalking;
        }
    }

    WanderingStateData Data { get { return (WanderingStateData)base._data; } }

}