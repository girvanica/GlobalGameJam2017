using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    bool _startAnimation;
    float _time;
    GameObject _platform;

    // Use this for initialization
    void Start () {
        _platform = GameObject.Find("Platform");
    }
	
	// Update is called once per frame
	void Update () {
        if (_startAnimation) {
            var player = GameObject.Find("Player");
            if (Time.realtimeSinceStartup - _time < 2 )
            {
                Vector3 pos = _platform.transform.position;
                pos.y += Time.deltaTime;
                transform.position = pos;
            }
            else
            {
                _startAnimation = false;
            }
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            if (col.gameObject.GetComponent<PlayerKey>().NumberOfKeys > 0)
            {
                _time = Time.realtimeSinceStartup;
                _startAnimation = true;
                col.gameObject.GetComponent<Player>().Goto(new Vector3(_platform.transform.position.x, col.gameObject.transform.position.y, _platform.transform.position.z));
                var enemies = GameObject.FindObjectsOfType<Enemy>();
                foreach (var e in enemies) {
                    e.HasStopped = true;
                }
            }
        }
    }
}
