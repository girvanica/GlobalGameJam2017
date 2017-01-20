using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    Rigidbody rig;
    Vector3 vel;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rig.MovePosition(rig.position + vel * Time.fixedDeltaTime);
	}

    public void Move(Vector3 _vel) {
        vel = _vel;
    }
}
