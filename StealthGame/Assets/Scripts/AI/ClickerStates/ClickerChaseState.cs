using System.Collections;
using UnityEngine;

public class ClickerChaseState : ChaseState, IOverrideState
{
    public ClickerChaseState(IEntityContainer entityContainer)
        : base(entityContainer)
    {
    }

    public override IEnumerator OnStateExecute(StateMachine stateMachine)
    {
        var soundSource = _entityContainer.GetEntity<SoundSourceEntity>();
        var noise = _entityContainer.GetEntity<NoiseProducerEntity>();
        var agent = _entityContainer.GetEntity<NavMeshAgentEntity>();
        agent.IsStopped = true;
        if (soundSource != null)
        {
            soundSource.PlaySound(SoundTypes.ClickerScream);
        }
        noise.NoiseLevel += _data.RoarExtraNoise;
        noise.Type = NoiseType.ClickerScream;
        yield return new WaitForSeconds(2f);
        noise.Type = NoiseType.Footsteps;

        yield return stateMachine.StartCoroutine(base.OnStateExecute(stateMachine));
    }

}