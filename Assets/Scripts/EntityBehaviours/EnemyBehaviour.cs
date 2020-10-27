using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonMover))]
public class EnemyBehaviour : MonoBehaviour, IEntity
{
    private NavMeshAgent enemyNavMeshAgent;
    private ThirdPersonMover mover;

    private IEntityAction activeAction;

    void Start()
    {
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        mover = GetComponent<ThirdPersonMover>();

        enemyNavMeshAgent.updateRotation = false;
    }

    void Update()
    {
        //TODO: convert me to actions, likely with an AI controller that gives them out, much like the CommandController
        _checkForNextMove();
        _updateAnimation();
    }

    public void AssignNewAction(IEntityAction newAction)
    {
        if (activeAction != null && !activeAction.IsDone()) activeAction.Abort();

        activeAction = newAction;
    }

    public ThirdPersonMover GetMover()
    {
        return mover;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return enemyNavMeshAgent;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public TargetingEffect GetTargeting()
    {
        return null;
    }

    public Transform GetWeaponHolder()
    {
        return transform.Find("Rig/WeaponHolder");
    }

    private void _checkForNextMove()
    {
        if (enemyNavMeshAgent.hasPath)
        {
            return;
        }

        enemyNavMeshAgent.SetDestination(transform.position + new Vector3(Random.value * 10f - 5f, 0, Random.value * 10f - 5f));
    }

    private void _updateAnimation()
    {
        if (enemyNavMeshAgent.remainingDistance > enemyNavMeshAgent.stoppingDistance)
        {
            mover.Move(enemyNavMeshAgent.desiredVelocity, false, false);
        }
        else
        {
            mover.Move(Vector3.zero, false, false);
        }
    }
}
