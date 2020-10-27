using UnityEngine;
using UnityEngine.AI;

public interface IEntity
{
    Vector3 GetPosition();
    TargetingEffect GetTargeting();
    NavMeshAgent GetNavMeshAgent();
    ThirdPersonMover GetMover();
    void AssignNewAction(IEntityAction newAction);
    Transform GetWeaponHolder();
    float GetAimRepositionAngle();
    float GetAimAngleLimit();
    Transform GetTransform();
}