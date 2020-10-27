using UnityEngine;
using UnityEngine.AI;

public class NavMoverAction : IEntityAction
{
    private Vector3 destination;
    private IEntity toMove;
    private bool validSetup;

    private NavMeshAgent navMeshAgent;
    private ThirdPersonMover mover;

    private float stopDistance = 0f;

    public NavMoverAction(Vector3 destination, IEntity toMove, float stopDistance = 0f)
    {
        this.destination = destination;
        this.toMove = toMove;
        this.stopDistance = stopDistance;

        validSetup = toMove != null;
        if (!validSetup) return;

        navMeshAgent = toMove.GetNavMeshAgent();
        mover = toMove.GetMover();

        validSetup = navMeshAgent != null;
    }

    public void Abort()
    {
        destination = toMove.GetPosition();
        navMeshAgent.SetDestination(destination);
        mover.Move(Vector3.zero, false, false);
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
        return navMeshAgent.remainingDistance <= (navMeshAgent.stoppingDistance + stopDistance);
    }
}