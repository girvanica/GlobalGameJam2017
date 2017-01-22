using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform : MonoBehaviour
{

    bool _startAnimation;
    float _time;
    GameObject _platform;
    public event System.Action OnEscape;

    // Use this for initialization
    void Start()
    {
        _platform = GameObject.Find("Platform");
    }

    // Update is called once per frame
    void Update()
    {
        if (_startAnimation)
        {
            if (Time.timeSinceLevelLoad - _time < 3)
            {
                Vector3 pos = transform.position;
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
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player>().hasKey)
            {
                _time = Time.realtimeSinceStartup;
                _startAnimation = true;
                col.gameObject.GetComponent<Player>().Goto(new Vector3(transform.position.x, col.gameObject.transform.position.y, transform.position.z));
                var enemies = GameObject.FindObjectsOfType<Enemy>();
                foreach (var e in enemies)
                {
                    e.HasStopped = true;
                }
            }
        }
    }
}
