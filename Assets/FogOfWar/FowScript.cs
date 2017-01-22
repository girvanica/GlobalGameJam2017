using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowScript : MonoBehaviour {

    public float _FogRadius = 11.0f;
    public float _FogMaxRadius = 11.0f;
	public float _FogMinRadius = 1.0f;
	public float pulseMaxRadius = 11.0f;
	public float pulseMinRadius = 11.0f;
	public float pulseSignal = 0.2f;
    public float signal = -1.5f;
    bool _isPlaying = false;
    bool _canBreath = true;
    float _time;

	//pulse fired audio vars
	bool isPulseBreathPlaying;

    GameObject player;
    PlayerPulse pulse;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        pulse = ((Player)player.GetComponent<Player>()).GetComponent<PlayerPulse>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            UpdateMaterial(player);
        }

		bool startAudio = false;
        if (_canBreath)
        {
			
			if (pulse.isPulseFiring) {
				
				if (!_isPlaying) {
					startAudio = true;
				} else {
					if (!isPulseBreathPlaying) {
						player.GetComponent<AudioSource> ().Stop ();
						startAudio = true;
						isPulseBreathPlaying = true;
					}
				}

				float fromValue = _FogRadius;
				float toValue = _FogMaxRadius;

				toValue = pulse.getIsFading () ? pulseMinRadius : pulseMaxRadius;

				float nextStep = Utility.Approach (fromValue, toValue, pulseSignal);
				//Debug.Log ("next step is " + nextStep);
				_FogRadius = nextStep;
			} else {
				_FogRadius += 0.02f * signal;
				if (_FogRadius < 2) {
					signal = 1.4f;
					if (!_isPlaying) {
						startAudio = true;
						_isPlaying = true;
					}
				}
				if (_FogRadius > 4.5) {
					signal = -1.5f;

					_isPlaying = false;
					//player.GetComponent<AudioSource>().Stop();
					//_canBreath = false;
				}
			}
        }
        /*if (Time.realtimeSinceStartup - _time >= 3)
        {
            
        }*/

		if (startAudio) {
			_time = Time.realtimeSinceStartup;
			if (player != null)
				player.GetComponent<AudioSource> ().Play ();
		}

        if (Time.realtimeSinceStartup - _time >= 5)
        {
			isPulseBreathPlaying = false;
            _canBreath = true;
        }
    }

    public void UpdateMaterial(GameObject player)
    {
        var material = GetComponent<Renderer>().sharedMaterial;
        if (material != null)
        {
            material.SetFloat("_FogRadius", _FogRadius);
            material.SetFloat("_FogMaxRadius", _FogMaxRadius);
            material.SetVector("_PlayerPos", player.transform.position);
        }
    }
}
