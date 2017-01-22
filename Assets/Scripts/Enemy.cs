using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State { Idle, Chasing, Attacking, Baited };
    State currentState;

    NavMeshAgent pathFinder;
    Transform target;
    LivingEntity targetEntity;
    Player targetPlayer;
    GameObject _player;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    public float damage;
    public float moveDuration;
    float moveTime;
    float nextAttackTime;

    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;
    public bool HasStopped = false;
    bool doUpdate = true;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player != null)
        {
            target = _player.transform;
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

        damage = 10 * PlayerPrefs.GetInt("Difficulty");
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

        moveTime = Time.timeSinceLevelLoad + moveDuration;
    }


    void OnTargetDrop()
    {
        hasTarget = true;
        currentState = State.Baited;
        moveTime = Time.timeSinceLevelLoad + moveDuration;
    }


    // Update is called once per frame
    void Update()
    {
        if (!doUpdate)
            return;
        pathFinder = GetComponent<NavMeshAgent>();

        if (_player != null)
        {
            if (this.currentState == State.Chasing)
            {
                if ((this.transform.position - _player.transform.position).sqrMagnitude < 60)
                    this.currentState = State.Idle;
            }

            if (this.currentState == State.Idle)
            {
                if ((this.transform.position - _player.transform.position).sqrMagnitude < 30)
                    OnTargetPulse();
            }
        }
        else
        {
            StopAllCoroutines();
        }

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

    void OnDisable()
    {
        doUpdate = false;
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

            if (HasStopped)
            {
                this.StopAllCoroutines();
            }
            currentState = State.Chasing;
            pathFinder.enabled = true;

            yield return null;
        }
    }

    IEnumerator updatePath()
    {
        float refreshRate = .25f;

        while (hasTarget && target != null && transform != null)
        {
            if (currentState == State.Chasing)
            {
                while (Time.timeSinceLevelLoad < moveTime)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                    if (!dead)
                    {
                        pathFinder.SetDestination(targetPosition);
                        yield return new WaitForSeconds(refreshRate);
                    }
                }
                pathFinder.SetDestination(transform.position);
            }
            if (currentState == State.Baited)
            {
                while (Time.timeSinceLevelLoad < moveTime)
                {
                    pathFinder.SetDestination(targetPlayer.dropLocation);
                    yield return new WaitForSeconds(refreshRate);
                }
            }
            hasTarget = false;
            currentState = State.Idle;
        }
    }
}
