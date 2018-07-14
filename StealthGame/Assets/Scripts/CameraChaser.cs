using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChaser : MonoBehaviour {

    public Transform Target;
    public float speed = 0.3f;

	// Use this for initialization
	void Start () {
        GameObject.FindWithTag("")
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.Lerp(this.transform.position, Target.position, speed);	
	}
}
