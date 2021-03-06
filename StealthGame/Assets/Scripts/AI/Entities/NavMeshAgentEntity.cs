using UnityEngine;
using UnityEngine.AI;

public interface INavMeshAgent : IEntity
{
    bool IsStopped { get; set; }
    float MovementSpeed { get; set; }
    bool IsDestinationReached();
    void SetDestination(Vector3 destination);
    Vector3 GetClosestToNavMeshPoint(Vector3 point);
}

public class NavMeshAgentEntity : INavMeshAgent
{
    private readonly NavMeshAgent _agent;

    public NavMeshAgentEntity(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public float MovementSpeed { get { return _agent.speed; } set { _agent.speed = value; } }

    public bool IsStopped { get { return _agent.isStopped; } set { _agent.isStopped = value; } }

    public bool IsDestinationReached()
    {
        return _agent.remainingDistance <= _agent.stoppingDistance
            && (!_agent.hasPath || Mathf.Approximately(_agent.velocity.sqrMagnitude, 0f));
    }

    public void SetDestination(Vector3 destination)
    {
        IsStopped = false;
        _agent.SetDestination(destination);
    }

    public Vector3 GetClosestToNavMeshPoint(Vector3 point)
    {
        NavMeshHit hit;
        if (_agent.Raycast(point, out hit))
        {
            return hit.position;
        }
        return point;
    }
}