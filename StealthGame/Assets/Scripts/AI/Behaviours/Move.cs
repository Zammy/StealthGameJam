using System.Collections;
using UnityEngine;

public static class Move
{
    public static IEnumerator Do(IEntityContainer entityContainer, Vector3 destination)
    {
        var agent = entityContainer.GetEntity<INavMeshAgent>();

        agent.SetDestination(destination);
        yield return null;

        while (!agent.IsDestinationReached())
        {
            yield return null;
        }
    }
}