using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPulse : MonoBehaviour {

	GameObject lightGameObject;
	Light lightComp;
	public float lightIntensity;

	// Use this for initialization
	void Start () {

		// Code to add a light source to the scene
		lightGameObject = new GameObject("TheLight");
		Light lightComp = lightGameObject.AddComponent<Light> ();
		lightComp.color = Color.blue;
		lightIntensity = 100.0f;
		lightComp.intensity = lightIntensity;
		lightGameObject.transform.position = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// Sets the position of the generated light to the current player position
		lightGameObject.transform.position = this.transform.position;

	}
}
