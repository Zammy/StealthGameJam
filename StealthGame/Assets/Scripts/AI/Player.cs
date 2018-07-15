﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Player : MonoBehaviour
{
    [Header("State")]
    public float NoiseLevel = 100;
    public bool StealthMode;
    public bool Moving;
    public bool DebugMode = true;
    public int Peeking = -1;

    [Header("Settings")]
    public float EyesightFOV = 90;
    public float EyesightDistance = 25;
    public float StealthSpeed = 1;
    public float RunSpeed = 2.5f;
    public float clickTime = .25f;
    public float HeadTurnSpeed = .25f;
    public float PeekingDistance = 2f;


    [Header("References")]
    public Collider BodyCollider;
    public Transform Head;
    public GameObject LookAhead;
    public GameObject LookatTarget;
    public Transform PeekLeft;
    public Transform PeekRight;
    public Transform PeekUp;
    public Transform PeekCenter;

    private Vector3 LastPos;
    private float Speed;
    private float lastClickTime = 0;
    private int ClickCounter=0;
    private Vector3 destination;

    
    
    public bool LookatMode = false;
    
    void Start()
    {
        var entityContainer = GetComponent<IEntityContainer>();
        entityContainer.AddEntity(new NavMeshAgentEntity(GetComponent<NavMeshAgent>()));
        entityContainer.AddEntity(new PhysicalEntity(transform));
        entityContainer.AddEntity(new EyesightEntity(EyesightFOV, EyesightDistance, Head));
        entityContainer.AddEntity(new SpottableEntity(BodyCollider));


        ServiceLocator.Instance.GetService<IVisibilitySystem>().AddPlayer(entityContainer);

        LookatTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        LookatTarget.name = "LookatPos";
        LookatTarget.GetComponent<SphereCollider>().enabled = false;
        LookatTarget.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
        LookatTarget.transform.position = LookAhead.transform.position;
        if (!DebugMode)
        {
            LookatTarget.GetComponent<MeshRenderer>().enabled = false;
            PeekLeft.GetComponent<MeshRenderer>().enabled = false;
            PeekRight.GetComponent<MeshRenderer>().enabled = false;
            PeekCenter.GetComponent<MeshRenderer>().enabled = false;
            PeekUp.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    Coroutine _goto;

    void Update()
    {
        //LOOKAT RMB
        LookatMode = false;
        if (Input.GetMouseButton(1))
        {
           
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.GetMask("Ground")))
            {   
                LookatTarget.transform.position = new Vector3(hitInfo.point.x, Head.transform.position.y, hitInfo.point.z);
                if (StealthMode || !Moving) LookatMode = true;
            }
        }

        if (LookatMode)
        {
            //Vector3 direction = LookatTarget.transform.position - Head.transform.position;
            //Quaternion toRotation = Quaternion.FromToRotation(Head.transform.forward, direction);
            //Head.transform.rotation = Quaternion.Lerp(Head.transform.rotation, toRotation, HeadTurnSpeed * Time.time);
            Head.transform.LookAt(LookatTarget.transform.position);
           // if (Mathf.Abs(Head.transform.localEulerAngles.y) > 90) LookatMode = false;
        }
        else
        {
            Head.transform.LookAt(LookAhead.transform.position);
        }

        //MOVING
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
            //GetComponent<Animator>().SetBool("Stealth", true);
        }

        //if (Moving && !StealthMode) LookatMode = false;

        NoiseLevel = 100 * (StealthMode ? 0.3f : 1) * (Moving ? 1 : 0);

        if (Input.GetMouseButtonDown(0))
        {

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.GetMask("Ground")))
            {
                destination = hitInfo.point;
            }

            if (Time.time - lastClickTime < clickTime && ClickCounter == 1)
            {
                ClickCounter = 2;
            }
            else
            {
                ClickCounter = 1;
            }

            lastClickTime = Time.time;
        }

        if (ClickCounter == 2)
        {
            GetComponent<NavMeshAgent>().speed = RunSpeed;
            StealthMode = false;
            GetComponent<Animator>().SetBool("Stealth", false);

            if (_goto != null)
            {
                StopCoroutine(_goto);
                _goto = null;
            }
            _goto = StartCoroutine(Move.Do(GetComponent<IEntityContainer>(), destination));

            Debug.Log("Double click.");
            ClickCounter = 0;
        }

        if (ClickCounter == 1 && Time.time - lastClickTime > clickTime)
        {
            GetComponent<NavMeshAgent>().speed = StealthSpeed;
            StealthMode = true;
            GetComponent<Animator>().SetBool("Stealth", true);

            if (_goto != null)
            {
                StopCoroutine(_goto);
                _goto = null;
            }
            _goto = StartCoroutine(Move.Do(GetComponent<IEntityContainer>(), destination));

            Debug.Log("Single Click.");
            ClickCounter = 0;
        }

        LastPos = this.transform.position;

        //PEEK
        Peeking = -1;
        if (!Moving && LookatMode)
        {
            Ray RayCenter = new Ray(PeekCenter.position, PeekCenter.forward);
            Ray RayLeft = new Ray(PeekLeft.position, PeekLeft.forward);
            Ray RayRight = new Ray(PeekRight.position, PeekRight.forward);
            Ray RayUp = new Ray(PeekUp.position, PeekUp.forward);
            
            RaycastHit hitInfo;

            if (Physics.Raycast(RayCenter, out hitInfo, PeekingDistance))
            {
                if (hitInfo.collider.tag == "VisibilityObstacle")
                {
                    Debug.Log("Peek: Center Hit.");
                    Peeking = 0;

                    //Left test
                    if (Physics.Raycast(RayLeft, out hitInfo, PeekingDistance))
                    {
                        if (hitInfo.collider.tag == "VisibilityObstacle")
                        {
                            Debug.Log("Peeking: Left Hit.");
                        }
                        else
                        {
                            Debug.Log("Peeking: Left free.");
                            Peeking = 1;
                        }
                    }

                    //Right test
                    if (Physics.Raycast(RayRight, out hitInfo, PeekingDistance))
                    {
                        if (hitInfo.collider.tag == "VisibilityObstacle")
                        {
                            Debug.Log("Peeking: Right Hit.");
                        }
                        else
                        {
                            Debug.Log("Peeking: Right free.");
                            Peeking = 2;
                        }
                    }

                    //Up test
                    if (Physics.Raycast(RayUp, out hitInfo, PeekingDistance))
                    {
                        if (hitInfo.collider.tag == "VisibilityObstacle")
                        {
                            Debug.Log("Peeking: Up Hit.");
                        }
                        else
                        {
                            Debug.Log("Peeking: Up free.");
                            Peeking = 3;
                        }
                    }
                }
            }
        }
    }
}