using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject target; //target to follow
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position;
        transform.Translate(new Vector3(0, 0, -5));
	}
}
