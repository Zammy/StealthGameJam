using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Player : MonoBehaviour
{
    [Header("State")]
    public float NoiseLevel;
    public bool StealthMode;

    [Header("Settings")]
    public float EyesightFOV = 90;
    public float EyesightDistance = 25;

    [Header("References")]
    public Transform Head;

    void Start()
    {
        var entityContainer = GetComponent<IEntityContainer>();
        entityContainer.AddEntity(new NavMeshAgentEntity(GetComponent<NavMeshAgent>()));
        entityContainer.AddEntity(new PhysicalEntity(transform));
        entityContainer.AddEntity(new EyesightEntity(EyesightFOV, EyesightDistance, Head));

        ServiceLocator.Instance.GetService<IVisibilitySystem>()
            .AddEntity(entityContainer);
    }

    Coroutine _goto;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_goto != null)
            {
                StopCoroutine(_goto);
                _goto = null;
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.GetMask("Ground")))
            {
                var destination = hitInfo.point;
                _goto = StartCoroutine(Move.Do(GetComponent<IEntityContainer>(), destination));
            }
        }
    }
}