using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowScript : MonoBehaviour {

    public float _FogRadius = 11.0f;
    public float _FogMaxRadius = 1.0f;
    float signal = -1.5f;
    bool _isPlaying = false;
    bool _canBreath = true;
    float _time;

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.Find("Player");
        UpdateMaterial(player);
        if (_canBreath)
        {
            _FogRadius += 0.02f * signal;
            if (_FogRadius < 7)
            {
                signal = 1.4f;
                if (!_isPlaying)
                {
                    _isPlaying = true;
                    _time = Time.realtimeSinceStartup;
                    player.GetComponent<AudioSource>().Play();
                }
            }
            if (_FogRadius > 9.5)
            {
                signal = -1.5f;

                _isPlaying = false;
                //player.GetComponent<AudioSource>().Stop();
                //_canBreath = false;
            }
        }
        /*if (Time.realtimeSinceStartup - _time >= 3)
        {
            
        }*/

        if (Time.realtimeSinceStartup - _time >= 5)
        {
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
