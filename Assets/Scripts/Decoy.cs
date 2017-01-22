using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{ 
    public event System.Action OnDecoyPickup;

    void OnCollisionEnter(Collision col)
    {
        print(col.gameObject.tag);
        if (col.gameObject.tag == "Enemy")
        {
            // Play sound
            print("col");
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
