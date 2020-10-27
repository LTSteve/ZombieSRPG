using UnityEngine;
using UnityEngine.AI;

public class InteractAction : IEntityAction
{
    private IInteractable toInteractWith;
    private IEntity interactor;
    private bool doneInteracting = false;

    public InteractAction(IInteractable interactable, IEntity entity)
    {
        toInteractWith = interactable;
        interactor = entity;
        doneInteracting = interactable == null;
    }

    public void Abort()
    {
        doneInteracting = true;
    }

    public bool IsDone()
    {
        return doneInteracting;
    }

    public void Update()
    {
        toInteractWith.Interact(interactor);
        doneInteracting = true;
    }
}