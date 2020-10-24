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

    public static Vector3? GetDestination()
    {
        if (Instance == null) return null;
        return Instance.destinationSet ? (Vector3?)Instance.destination : null;
    }

    public static void UnsetDestination()
    {
        if (Instance == null) return;
        Instance.destinationSet = false;
    }

    private Vector3 destination;
    private bool destinationSet = false;

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
            destination = hit.point;
            destinationSet = true;
        }
    }
}
