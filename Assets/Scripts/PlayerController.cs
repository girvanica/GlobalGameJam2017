using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    Rigidbody rig;
    Vector3 vel;
    Vector3 lookDirection = Vector3.zero;
    public float rotationSpeed = 7;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rig.MovePosition(rig.position + vel * Time.fixedDeltaTime);
        //Look in the direction player is moving.
    }

    public void Move(Vector3 _vel, bool rotate) {
        vel = _vel;
        if(rotate)
        {
            lookDirection = -vel;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
