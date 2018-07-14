using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieInit : MonoBehaviour
{
    void Start()
    {
        var entityContainer = GetComponent<IEntityContainer>();
        entityContainer.AddEntity(new NavMeshAgentEntity(GetComponent<NavMeshAgent>()));
        entityContainer.AddEntity(new PhysicalEntity(transform));

        var stateMachine = GetComponent<StateMachine>();
        stateMachine.AddState(new WanderingState(entityContainer));
        stateMachine.ChangeState(typeof(WanderingState));
    }
}
