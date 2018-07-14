using System.Collections;
using UnityEngine;

public class WanderingState : ISMState
{
    readonly IEntityContainer _entityContainer;

    WanderingStateData _data;

    public WanderingState(IEntityContainer entityContainer)
    {
        _entityContainer = entityContainer;
    }

    public void SetData(SMStateData data)
    {
        _data = (WanderingStateData)data;
    }

    public void OnStateEnter()
    {
    }

    public IEnumerator OnStateExecute(StateMachine stateMachine)
    {
        var physicalEntity = _entityContainer.GetEntity<IPhysicalEntity>();
        var agent = _entityContainer.GetEntity<INavMeshAgent>();
        while (true)
        {
            float distance = Random.Range(_data.MinDistance, _data.MaxDistance);
            Vector3 dest = physicalEntity.Position.RandomPosAround(distance);
            dest = agent.GetClosestToNavMeshPoint(dest);
            yield return Move.Do(_entityContainer, dest);

            yield return new WaitForSeconds(Random.Range(_data.MinWait, _data.MaxWait));
        }
    }

    public void OnStateExit()
    {
    }
}