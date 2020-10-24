using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent playerNavMeshAgent;
    [SerializeField] private ThirdPersonMover mover;

    private IEntityAction activeAction;

    private void Start()
    {
        playerNavMeshAgent.updateRotation = false;

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
}
