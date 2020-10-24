using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    public Camera PlayerCamera;
    public NavMeshAgent PlayerNavMeshAgent;

    public CrouchlessThirdPersonCharacter Character;

    void Start()
    {
        PlayerCamera = Camera.main;

        PlayerNavMeshAgent.updateRotation = false;
    }

    void Update()
    {
        _checkForMouseClick();

        _updateCharacterAnimation();
    }

    private void _checkForMouseClick()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        var clickRay = PlayerCamera.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(clickRay,out var hit))
        {
            return;
        }

        PlayerNavMeshAgent.SetDestination(hit.point);
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
