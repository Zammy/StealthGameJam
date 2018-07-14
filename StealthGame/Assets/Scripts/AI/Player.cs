using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Player : MonoBehaviour
{
    [Header("State")]
    public float NoiseLevel = 100;
    public bool StealthMode;
    public bool Moving;

    [Header("Settings")]
    public float EyesightFOV = 90;
    public float EyesightDistance = 25;
    public float StealthSpeed = 1;
    public float RunSpeed = 2.5f;
    public float clickTime = .25f;


    [Header("References")]
    public Transform Head;

    private Vector3 LastPos;
    private float Speed;

    void Start()
    {
        var entityContainer = GetComponent<IEntityContainer>();
        entityContainer.AddEntity(new NavMeshAgentEntity(GetComponent<NavMeshAgent>()));
        entityContainer.AddEntity(new PhysicalEntity(transform));
        entityContainer.AddEntity(new EyesightEntity(EyesightFOV, EyesightDistance, Head));

        //ServiceLocator.Instance.GetService<IVisibilitySystem>().AddEntity(entityContainer);
    }

    Coroutine _goto;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (StealthMode)
            {
                GetComponent<NavMeshAgent>().speed = RunSpeed;
                GetComponent<Animator>().SetBool("Stealth", false);
            }
            else
            {
                GetComponent<NavMeshAgent>().speed = StealthSpeed;
                GetComponent<Animator>().SetBool("Stealth", true);
            }
                

            StealthMode = !StealthMode;
        }

        Speed = Vector3.Distance(LastPos, this.transform.position) / Time.deltaTime;
        if (Speed > 0)
        {
            Moving = true;
            GetComponent<Animator>().SetBool("Moving", true);
        }
        else
        {
            Moving = false;
            GetComponent<Animator>().SetBool("Moving",false);
        }
        

        NoiseLevel = 100 * (StealthMode ? 0.3f : 1) * (Moving ? 1 : 0);



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

        LastPos = this.transform.position;
    }
}