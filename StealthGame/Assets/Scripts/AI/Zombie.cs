using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    Collider SpottableCollider;

    [SerializeField]
    GameObject Visual;

    void Start()
    {
        var entityContainer = GetComponent<IEntityContainer>();
        entityContainer.AddEntity(new NavMeshAgentEntity(GetComponent<NavMeshAgent>()));
        entityContainer.AddEntity(new PhysicalEntity(transform));
        entityContainer.AddEntity(new EnemySpottableEntity(Visual, SpottableCollider));

        var stateMachine = GetComponent<StateMachine>();
        stateMachine.AddState(new WanderingState(entityContainer));
        stateMachine.ChangeState(typeof(WanderingState));

        ServiceLocator.Instance.GetService<IVisibilitySystem>()
            .AddEnemy(entityContainer);
    }
}
