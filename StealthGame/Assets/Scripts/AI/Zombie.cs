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
    [SerializeField] float NoiseDistanceDiminution = 12f;
    [SerializeField] float BaseNoiseLevel = 50f;

    [Header("Sounds")]
    [SerializeField] AudioSource WalkingSound;
    [SerializeField] AudioSource RoarSound;

    void Start()
    {
        var entityContainer = GetComponent<IEntityContainer>();
        entityContainer.AddEntity(new NavMeshAgentEntity(GetComponent<NavMeshAgent>()));
        entityContainer.AddEntity(new PhysicalEntity(transform));
        entityContainer.AddEntity(new EnemySpottableEntity(Visual, SpottableCollider));
        entityContainer.AddEntity(new EnemyEyesightEntity(EyesightFOV, EyesightDistance, Head));
        entityContainer.AddEntity(new HearingEntity(NoiseDistanceDiminution));
        entityContainer.AddEntity(new NoiseProducerEntity() { NoiseLevel = BaseNoiseLevel });
        entityContainer.AddEntity(new SoundSourceEntity(new Dictionary<SoundTypes, AudioSource>()
        {
            { SoundTypes.Walking, WalkingSound},
            { SoundTypes.Roar, RoarSound}
        }));

        var stateMachine = GetComponent<StateMachine>();
        stateMachine.AddState(new WanderingState(entityContainer));
        stateMachine.AddState(new ChaseState(entityContainer));
        stateMachine.AddState(new SearchLocationState(entityContainer));
        stateMachine.ChangeState(typeof(WanderingState));

        ServiceLocator.Instance.GetService<IVisibilitySystem>()
            .AddEnemy(entityContainer);
        ServiceLocator.Instance.GetService<IHearingSystem>()
            .AddEnemy(entityContainer);

    }
}
