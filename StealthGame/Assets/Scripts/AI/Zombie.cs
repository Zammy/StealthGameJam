using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    Collider SpottableCollider;

    [SerializeField]
    Transform Head;

    [SerializeField]
    GameObject Visual;


    [Header("Settings")]
    [SerializeField] float EyesightFOV = 90;
    [SerializeField] float EyesightDistance = 25f;

    void Start()
    {
        var entityContainer = GetComponent<IEntityContainer>();
        entityContainer.AddEntity(new NavMeshAgentEntity(GetComponent<NavMeshAgent>()));
        entityContainer.AddEntity(new PhysicalEntity(transform));
        entityContainer.AddEntity(new EnemySpottableEntity(Visual, SpottableCollider));
        entityContainer.AddEntity(new EnemyEyesightEntity(EyesightFOV, EyesightDistance, Head));

        var stateMachine = GetComponent<StateMachine>();
        stateMachine.AddState(new WanderingState(entityContainer));
        stateMachine.AddState(new ChaseState(entityContainer));
        stateMachine.ChangeState(typeof(WanderingState));

        ServiceLocator.Instance.GetService<IVisibilitySystem>()
            .AddEnemy(entityContainer);
    }
}
