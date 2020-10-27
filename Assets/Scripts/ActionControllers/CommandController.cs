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

    public static List<IEntity> Selected = new List<IEntity>();

    void Update()
    {
        _checkForMouseClick();
    }

    private void _checkForMouseClick()
    {
        if (!Input.GetMouseButtonDown(0) || Camera.main == null)
        {
            return;
        }

        var clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(clickRay, out var hit))
        {
            foreach(var selected in Selected)
            {
                var clickedOn = hit.transform;

                var interactable = clickedOn.GetComponent<IInteractable>();
                var entity = clickedOn.GetComponent<IEntity>();

                if (interactable != null)
                {
                    var actionList = new EntityActionList();
                    var interactionRadius = interactable.GetInteractionRadius();
                    if (interactionRadius != -1) 
                    {
                        //close the distance first
                        actionList.AddAction(new NavMoverAction(hit.point, selected, interactionRadius));
                    }

                    actionList.AddAction(new InteractAction(interactable, selected));

                    selected.AssignNewAction(actionList);
                }
                else if(entity != null)
                {
                    if(entity != selected)
                    {
                        selected.AssignNewAction(new LookAtAction(hit.transform, selected.GetTargeting()));
                    }
                }
                else //ground
                {
                    selected.AssignNewAction(new NavMoverAction(hit.point, selected));
                }
            }
        }
    }
}
