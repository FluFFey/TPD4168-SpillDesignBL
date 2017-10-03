using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 newVelocity = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.W))
        {
            newVelocity.y += 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.S))
        {
            newVelocity.y -= 1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A))
        {
            newVelocity.x -= 1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
        {
            newVelocity.x += 1;
        }

        newVelocity = newVelocity.normalized;

        gameObject.GetComponent<Rigidbody2D>().velocity = newVelocity;

    }
}
