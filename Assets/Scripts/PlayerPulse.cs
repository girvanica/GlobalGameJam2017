using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPulse : MonoBehaviour {

	GameObject lightGameObject;
	public Light lightComp;
	public float lightRange = 100f;
	public float pulseTime = 10f;

	public long pulseTimeLength = 1000L;
	public float maxLightIntensity = 40f;
	public float minLightIntensity = 0f;

	public float maxDist = 30.0f;
	public float speed = 200f;

	private float currentIntensity;
	private float currentDistance;
	private float currentRange;

	bool isPulsing;
	bool isFading;

	float fadeFrom;
	float fadeTo;
	public float fadeRate = 1f;
	public Color color;
	float pulseStartTimeDelta;
	float pulseEndTimeDelta;
	public float offset;
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
		lightGameObject.transform.position = new Vector3 (this.transform.position.x, offset, this.transform.position.z);//this.transform.position;

		if (Input.GetKeyUp ("space")) {
			isPulsing = true;

			pulseStartTimeDelta = Time.deltaTime;
			isPulsing = true;
//			LightPulse ();
		} else {
//			isPulsing = false;
//			LightPulse ();
		}

		if (isPulsing) {
			LightPulse ();
		}
	}


	void LightPulse(){
		lightComp.color = color;
		lightComp.enabled = true;

		float deltaFade = fadeRate;// * Time.deltaTime;
		float nextStep = currentIntensity;
		float rangeStep = currentRange;
		if (isFading) {
//			nextStep = Mathf.Lerp (currentIntensity, minLightIntensity, deltaFade);
			nextStep = Approach (currentIntensity, minLightIntensity, deltaFade);
			rangeStep = Approach (currentRange, 0, deltaFade);
		} else {
//			nextStep = Mathf.Lerp (currentIntensity, maxLightIntensity, deltaFade);
			nextStep = Approach (currentIntensity, maxLightIntensity, deltaFade);
			rangeStep = Approach (currentRange, lightRange, deltaFade);
		}

		currentIntensity = nextStep;
		currentRange = rangeStep;

		lightComp.intensity = currentIntensity;
		lightComp.range = currentRange;

		if (currentIntensity > maxLightIntensity - 1) {
			isFading = true;
		}

		if (isFading && currentIntensity < minLightIntensity + 1) {
			isFading = false;
			isPulsing = false;
			lightComp.enabled = false;

		}
//		if (isPulsing) {
//			lightComp.enabled = true;
//			fadeFrom = 40.0f;
//			fadeTo = 0f;
//			lightComp.intensity = Mathf.Lerp (fadeFrom, fadeTo, pulseStartTimeDelta * fadeRate);
////			isPulsing = false;
//		} else {
//			//lightComp.enabled = true;
//			fadeFrom = 40.0f;
//			fadeTo = 0f;
//			lightComp.intensity = Mathf.InverseLerp (fadeFrom, fadeTo, pulseStartTimeDelta * fadeRate);
//			lightComp.enabled = false;
//			print (isPulsing);
//		}
	}

	float Approach (float fromValue, float toValue, float dt) {

		//get how much left
		float diff = toValue - fromValue;

		//we haven't reached it
		if (diff > dt) {
			return fromValue + dt;
		} 

		if (diff < -dt) {
			return fromValue - dt;
		}

		return toValue;

	}
}
