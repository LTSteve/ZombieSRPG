using UnityEngine;
using UnityEngine.AI;

public class NavMoverAction : IEntityAction
{
    private Vector3 destination;
    private Transform toMove;
    private bool validSetup;

    private NavMeshAgent navMeshAgent;
    private ThirdPersonMover mover;

    public NavMoverAction(Vector3 destination, Transform toMove)
    {
        if (toMove == null)
        {
            validSetup = false;
            return;
        }

        this.destination = destination;
        this.toMove = toMove;

        navMeshAgent = toMove.GetComponent<NavMeshAgent>();
        mover = toMove.GetComponent<ThirdPersonMover>();

        validSetup = mover != null && navMeshAgent != null;
    }

    public void Abort()
    {
        destination = toMove.position;
    }

    public bool IsDone()
    {
        if (!validSetup) return true;

        return _withinStoppingDistance();
    }

    public void Update()
    {
        if (!validSetup) return;

        _updateDestination();
        _updateCharacterAnimation();
    }

    private void _updateDestination()
    {
        navMeshAgent.SetDestination(destination);
    }

    private void _updateCharacterAnimation()
    {
        if (!_withinStoppingDistance())
        {
            mover.Move(navMeshAgent.desiredVelocity, false, false);
        }
        else
        {
            mover.Move(Vector3.zero, false, false);
        }
    }

    private bool _withinStoppingDistance()
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }
}