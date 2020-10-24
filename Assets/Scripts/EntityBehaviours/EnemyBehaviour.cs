using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent EnemyNavMeshAgent;
    public ThirdPersonMover Character;

    void Start()
    {
        EnemyNavMeshAgent.updateRotation = false;
    }

    void Update()
    {
        _checkForNextMove();
        _updateAnimation();
    }

    private void _checkForNextMove()
    {
        if (EnemyNavMeshAgent.hasPath)
        {
            return;
        }

        EnemyNavMeshAgent.SetDestination(transform.position + new Vector3(Random.value * 10f - 5f, 0, Random.value * 10f - 5f));
    }

    private void _updateAnimation()
    {
        if (EnemyNavMeshAgent.remainingDistance > EnemyNavMeshAgent.stoppingDistance)
        {
            Character.Move(EnemyNavMeshAgent.desiredVelocity, false, false);
        }
        else
        {
            Character.Move(Vector3.zero, false, false);
        }
    }
}
