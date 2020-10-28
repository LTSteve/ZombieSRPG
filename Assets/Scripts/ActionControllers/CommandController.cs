using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour
{
    #region Singleton
    private static CommandController Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion

    public static void HandleCommand(Vector3 point, Transform transform)
    {
        if (Instance == null) return;

        Instance._handleCommand(point, transform);
    }

    public static List<IEntity> Selected = new List<IEntity>();

    private void _handleCommand(Vector3 point, Transform target)
    {
        foreach (var selected in Selected)
        {
            var interactable = target.GetComponent<IInteractable>();
            var entity = target.GetComponent<IEntity>();

            if (interactable != null)
            {
                _assignInteractCommand(interactable, point, selected);
            }
            else if (entity != null)
            {
                _parseEntityCommand(entity, target, selected);
            }
            else //ground
            {
                _assignMoveCommand(point, selected);
            }
        }
    }

    private void _assignInteractCommand(IInteractable interactable, Vector3 point, IEntity selected)
    {
        var actionList = new EntityActionList();
        var interactionRadius = interactable.GetInteractionRadius();
        if (interactionRadius != -1)
        {
            //close the distance first
            actionList.AddAction(new NavMoverAction(point, selected, interactionRadius));
        }

        actionList.AddAction(new InteractAction(interactable, selected));

        selected.AssignNewAction(actionList);
    }

    private void _parseEntityCommand(IEntity otherEntity, Transform target, IEntity selected)
    {
        if (otherEntity != selected)
        {
            selected.AssignNewAction(new AimAndShootAction(target, selected, selected.GetWeaponHolder().GetComponentInChildren<IWeapon>()));
        }
    }

    private void _assignMoveCommand(Vector3 point, IEntity selected)
    {
        selected.AssignNewAction(new NavMoverAction(point, selected));
    }
}
