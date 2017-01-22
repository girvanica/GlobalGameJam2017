using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{ 
    public event System.Action OnDecoyPickup;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            // Play sound
            TriggerDecoyPickUp();
            Destroy(gameObject);
        }
    }

    void TriggerDecoyPickUp()
    {
        if (OnDecoyPickup != null)
        {
            OnDecoyPickup();
        }
    }
}
