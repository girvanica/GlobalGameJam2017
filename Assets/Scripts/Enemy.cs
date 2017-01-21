using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{

    NavMeshAgent pathFinder;
    Transform target;

    public bool HasStopped = false;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        pathFinder = GetComponent < NavMeshAgent >();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine (updatePath());
    }

    // Update is called once per frame
    void Update()
    {
        if (HasStopped) {
            this.StopAllCoroutines();
        }
    }

    IEnumerator updatePath()
    {
        float refreshRate = .25f;

        while (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            pathFinder.SetDestination(target.position);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
