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
        var soundSource = _entityContainer.GetEntity<SoundSourceEntity>();
        while (true)
        {
            if (soundSource != null)
            {
                soundSource.PlaySound(SoundTypes.Roar);
            }
            noise.NoiseLevel += Data.RoarExtraNoise;
            noise.Type = NoiseType.Roar;
            float wait = Random.Range(Data.MinWait, Data.MaxWait);
            yield return new WaitForSeconds(1f);
            wait -= 1f;
            noise.NoiseLevel -= Data.RoarExtraNoise;
            noise.Type = NoiseType.Footsteps;
            yield return new WaitForSeconds(wait);

            float distance = Random.Range(Data.MinDistance, Data.MaxDistance);
            Vector3 dest = physicalEntity.Position.RandomPosAround(distance);
            dest = agent.GetClosestToNavMeshPoint(dest);

            if (soundSource != null)
            {
                soundSource.PlaySound(SoundTypes.Walking);
            }
            noise.NoiseLevel += Data.WalkExtraNoise;
            noise.Type = NoiseType.Footsteps;

            yield return Move.Do(_entityContainer, dest);

            noise.NoiseLevel -= Data.WalkExtraNoise;
            noise.Type = NoiseType.Footsteps;
            if (soundSource != null)
            {
                soundSource.StopSound(SoundTypes.Walking);
            }
        }
    }

    WanderingStateData Data { get { return (WanderingStateData)base._data; } }

}