using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform : MonoBehaviour {

    bool _startAnimation;
    float _time;
    GameObject _platform;
    public event System.Action OnEscape;

    // Use this for initialization
    void Start () {
        _platform = GameObject.Find("Platform");
    }
	
	// Update is called once per frame
	void Update () {
        if (_startAnimation) {
            //var player = GameObject.FindGameObjectWithTag("Player");
            if (Time.timeSinceLevelLoad - _time < 2 )
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

    void EndGame()
    {
        var enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (var e in enemies)
        {
            e.HasStopped = true;
        }
        SceneManager.LoadScene("Menu");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player>().hasKey)
            {
                EndGame();
            }
        }
    }
}
