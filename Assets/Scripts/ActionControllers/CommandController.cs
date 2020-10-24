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

    public static List<PlayerBehaviour> Selected = new List<PlayerBehaviour>();

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
                selected.AssignNewAction(new NavMoverAction(hit.point, selected.transform));
            }
        }
    }
}
