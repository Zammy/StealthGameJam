using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChaser : MonoBehaviour
{
    public Transform Target;
    public float Speed = 0.1f;

    void Start()
    {
        if (Target == null)
        {
            Target = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, Target.position, Speed);
    }
}
