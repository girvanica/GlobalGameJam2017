using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State { Idle, Chasing, Attacking };
    State currentState;

    NavMeshAgent pathFinder;
    Transform target;
    LivingEntity targetEntity;
    Player targetPlayer;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    public float damage = 1;
    public float moveDuration = 5;
    float moveTime;
    float nextAttackTime;

    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;
    bool itemDropped;
    public bool HasStopped = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            targetPlayer = target.GetComponent<Player>();
            targetPlayer.OnTriggerPulse += OnTargetPulse;
            targetPlayer.OnTriggerDrop += OnTargetDrop;

        
            currentState = State.Idle;
            hasTarget = false;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            
        }

    }

    

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }


    void OnTargetPulse()
    {
        hasTarget = true;
        currentState = State.Chasing;
       // StartCoroutine(updatePath());
    }


    void OnTargetDrop()
    {
        itemDropped = true;
        currentState = State.Chasing;
      //  StartCoroutine(updatePath());
    }


    // Update is called once per frame
    void Update()
    {
      
        if (target != null)
        {
            StartCoroutine(updatePath());
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(attack());
                }
            }
        }


    }

    IEnumerator attack()
    {
        currentState = State.Attacking;
        pathFinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        bool hasAppliedDamage = false;
        while (percent <= 1)
        {

            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;


            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;
            if (HasStopped)
            {
                this.StopAllCoroutines();
            }
            currentState = State.Chasing;
            pathFinder.enabled = true;

        }

    }


        IEnumerator updatePath(){
            float refreshRate = .25f;
               
            while (hasTarget){
            
            if (currentState == State.Chasing){
                  
                //Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);

                float elapsedTime = 0;
               
                while (elapsedTime <= moveDuration)
                {
                    
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                    if (!dead)
                    {
                        //print("itemDropped: " + itemDropped);
                        if (itemDropped)
                        {
                            pathFinder.SetDestination(targetPlayer.dropLocation);
                            break;
                        }
                        else
                        {
                            pathFinder.SetDestination(targetPosition);
                        }
                        
                    }

                    elapsedTime ++;

                   yield return new WaitForSeconds(refreshRate);
                }
                
            }

            itemDropped = false;
            hasTarget = false;
            currentState = State.Idle;

            
        }
        }

    }


