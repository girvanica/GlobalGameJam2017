using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPulse : MonoBehaviour {

	GameObject lightGameObject;
	Light lightComp;
	public float lightRange = 100f;
	public float pulseTime = 10f;

	float maxDist = 30.0f;
	float speed = 200f;

	bool isPulsing;

	float fadeFrom;
	float fadeTo;
	float fadeRate;

	float pulseStartTimeDelta;
	float pulseEndTimeDelta;

	// Use this for initialization
	void Start () {

		// Code to add a light source to the scene
		lightGameObject = new GameObject("TheLight");
		lightComp = lightGameObject.AddComponent<Light> ();
		lightComp.color = Color.white;
		lightComp.range = lightRange;
		lightComp.enabled = isPulsing;
		lightGameObject.transform.position = this.transform.position;
        // Play pulse
        //var audioClip = Resources.Load<AudioClip>("ed_pulse_2");
        //AudioSource.PlayClipAtPoint(audioClip, new Vector3(5, 1, 2));
    }
	
	// Update is called once per frame
	void Update () {
		// Sets the position of the generated light to the current player position
		lightGameObject.transform.position = this.transform.position;

		if (Input.GetKeyUp ("space")) {
			isPulsing = true;

			pulseStartTimeDelta = Time.deltaTime;

			LightPulse ();
		} else {
			isPulsing = false;
			LightPulse ();
		}
	}


	void LightPulse(){
		if (isPulsing) {
			lightComp.enabled = true;
			fadeFrom = 40.0f;
			fadeTo = 0f;
			lightComp.intensity = Mathf.Lerp (fadeFrom, fadeTo, pulseStartTimeDelta * fadeRate);
			isPulsing = false;
		} else {
			//lightComp.enabled = true;
			fadeFrom = 40.0f;
			fadeTo = 0f;
			lightComp.intensity = Mathf.InverseLerp (fadeFrom, fadeTo, pulseStartTimeDelta * fadeRate);
			lightComp.enabled = false;
			//print (isPulsing);
		}
	}
}
