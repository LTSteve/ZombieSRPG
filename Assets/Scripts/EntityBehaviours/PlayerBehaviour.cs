using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerBehaviour : MonoBehaviour
{
    public NavMeshAgent PlayerNavMeshAgent;

    public ThirdPersonMover Character;

    void Start()
    {
        PlayerNavMeshAgent.updateRotation = false;
    }

    void Update()
    {
        _checkForNewDestination();

        _updateCharacterAnimation();
    }

    private void _checkForNewDestination()
    {
        var newDestination = CommandController.GetDestination();

        if (newDestination.HasValue)
        {
            PlayerNavMeshAgent.SetDestination(newDestination.Value);
        }
    }

    private void _updateCharacterAnimation()
    {
        if (PlayerNavMeshAgent.remainingDistance > PlayerNavMeshAgent.stoppingDistance)
        {
            Character.Move(PlayerNavMeshAgent.desiredVelocity, false, false);
        }
        else
        {
            Character.Move(Vector3.zero, false, false);
        }
    }
}
