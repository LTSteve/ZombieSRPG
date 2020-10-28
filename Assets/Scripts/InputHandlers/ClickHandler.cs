using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
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
            CommandController.HandleCommand(hit.point, hit.transform);
        }
    }
}
