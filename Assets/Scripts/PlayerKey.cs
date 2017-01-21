using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKey : MonoBehaviour {

    public int NumberOfKeys = 0;
    GameObject _player = GameObject.Find("Player");

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Key")
        {
            NumberOfKeys++;
            Destroy(col.gameObject);
        }
    }
}
