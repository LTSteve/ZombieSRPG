﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonMover))]
public class PlayerEntity : MonoBehaviour, IEntity
{
    [SerializeField]
    private float aimRepositionAngle = -1f;
    [SerializeField]
    private float angleLimit = -1f;

    private NavMeshAgent playerNavMeshAgent = null;
    private ThirdPersonMover mover;

    private IEntityAction activeAction;

    private void Start()
    {
        playerNavMeshAgent = GetComponent<NavMeshAgent>();
        mover = GetComponent<ThirdPersonMover>();

        /*
         * TEMPORARY HACK 
         * TODO: ADD BOX SELECT AND SELECTION STUFF
         */
        CommandController.Selected.Add(this);
    }

    private void Update()
    {
        if(activeAction != null)
        {
            activeAction.Update();
        }
    }

    public void AssignNewAction(IEntityAction newAction)
    {
        if (activeAction != null && !activeAction.IsDone()) activeAction.Abort();

        activeAction = newAction;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public TargetingEffect GetTargeting()
    {
        return transform.Find("Rig/AimTarget").GetComponent<TargetingEffect>();
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return playerNavMeshAgent;
    }

    public ThirdPersonMover GetMover()
    {
        return mover;
    }

    public Transform GetWeaponHolder()
    {
        return transform.Find("Rig/WeaponHolder/WeaponPivot");
    }

    public float GetAimRepositionAngle()
    {
        return aimRepositionAngle;
    }

    public float GetAimAngleLimit()
    {
        return angleLimit;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public EntityRigStatemachine GetRigStatemachine()
    {
        return GetComponentInChildren<EntityRigStatemachine>();
    }
}
