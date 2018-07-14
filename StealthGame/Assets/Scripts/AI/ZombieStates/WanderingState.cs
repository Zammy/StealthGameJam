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
        while (true)
        {
            float distance = Random.Range(_data.MinDistance, _data.MaxDistance);
            var dest = physicalEntity.Position.RandomPosAround(distance);

            yield return Move.Do(_entityContainer, dest);
        }
    }

    public void OnStateExit()
    {
    }
}