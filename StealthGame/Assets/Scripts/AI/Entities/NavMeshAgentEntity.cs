using UnityEngine;
using UnityEngine.AI;

public interface INavMeshAgent : IEntity
{
    bool IsDestinationReached();

    void SetDestination(Vector3 destination);
}

public class NavMeshAgentEntity : INavMeshAgent
{
    private readonly NavMeshAgent _agent;

    public NavMeshAgentEntity(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public bool IsDestinationReached()
    {
        return _agent.remainingDistance <= _agent.stoppingDistance
            && (!_agent.hasPath || Mathf.Approximately(_agent.velocity.sqrMagnitude, 0f));
    }

    public void SetDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }
}