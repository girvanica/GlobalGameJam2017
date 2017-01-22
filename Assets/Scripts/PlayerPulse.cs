using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	//pulse is firing, light is growing
	bool isPulsing;
	//pulse is growing, light is fading 
	bool isFading;
	public bool isPulseFiring;
	float fadeFrom;
	float fadeTo;
	public float fadeRate = 1f;
	public Color color;
	float pulseStartTimeDelta;
	float pulseEndTimeDelta;
	public float offset;
	// Use this for initialization
	void Start () {
		isPulseFiring = false;
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
        var pulseSlider = GameObject.FindGameObjectWithTag("PulseSlider").GetComponent<Slider>();

        if (Input.GetButtonDown("Jump") && pulseSlider.value == 100) {
			isPulsing = true;
			isPulseFiring = true;
			pulseStartTimeDelta = Time.deltaTime;

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
			nextStep = Utility.Approach (currentIntensity, minLightIntensity, deltaFade);
			rangeStep = Utility.Approach (currentRange, 0, deltaFade);
		} else {
//			nextStep = Mathf.Lerp (currentIntensity, maxLightIntensity, deltaFade);
			nextStep = Utility.Approach (currentIntensity, maxLightIntensity, deltaFade);
			rangeStep = Utility.Approach (currentRange, lightRange, deltaFade);
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
			isPulseFiring = false;
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

	public bool getIsPulsing() {
		return isPulsing;
	}

	public bool getIsFading() {
		return isFading;
	}
}
