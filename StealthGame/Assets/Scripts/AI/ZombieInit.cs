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
                Debug.LogFormat("destination {0}", destination);
                StartCoroutine(Test(destination));
            }
        }
    }

    IEnumerator Test(Vector3 destination)
    {
        _goto = StartCoroutine(Move.Do(GetComponent<IEntityContainer>(), destination));
        yield return _goto;
        Debug.Log("Reached");
    }
}
