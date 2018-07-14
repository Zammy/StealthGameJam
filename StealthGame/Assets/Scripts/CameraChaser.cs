using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChaser : MonoBehaviour {

    public Transform Target;
    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
        Target = GameObject.FindWithTag("Player").transform;
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.Lerp(this.transform.position, Target.position, speed);	
	}
}
