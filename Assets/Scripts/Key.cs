using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour {

    public int NumberOfKeys = 0;
    GameObject _key;
    public event System.Action OnKeyPickup;


    // Use this for initialization
    void Start () {
        _key = GameObject.FindGameObjectWithTag("Key");
    }

    // Update is called once per frame
    void Update () {
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            // Play sound
            var audioClip = Resources.Load<AudioClip>("ed_key");
            AudioSource.PlayClipAtPoint(audioClip, new Vector3(5, 1, 2));
            NumberOfKeys++;
            GameObject.FindGameObjectWithTag("Orb").GetComponent<Image>().color = Color.white;
            Destroy(_key);
            TriggerKeyPickUp();
        }
    }

    void TriggerKeyPickUp()
    {
        if (OnKeyPickup != null)
        {
            OnKeyPickup();
        }
    }
}
