using UnityEngine;

public interface IInteractable
{
    void Interact(IEntity interactor);
    Vector3 GetPosition();
    float GetInteractionRadius();
}