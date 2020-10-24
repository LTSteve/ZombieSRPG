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
                if(hit.transform.gameObject.layer == 10)
                {
                    selected.AssignNewAction(new LookAtAction(hit.transform, selected.transform.Find("Rig/AimTarget").GetComponent<TargetingEffect>()));
                }
                else
                {
                    selected.AssignNewAction(new NavMoverAction(hit.point, selected.transform));
                }
            }
        }
        /*
        var allHits = Physics.RaycastAll(clickRay);

        foreach(var raycastHit in allHits)
        {
            if (raycastHit.transform.gameObject.layer == 10)
            {
                Debug.Log("Entity");
            }
            else
            {
                Debug.Log("NotEntity");
            }
        }
        */
    }
}
