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


    private Vector3 raycastStart;
    private Vector3 raycastEnd;
    private bool drawGizmos = false;

    private void _checkForMouseClick()
    {
        if (!Input.GetMouseButtonDown(0) || Camera.main == null)
        {
            return;
        }

        var clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(clickRay, out var hit, LayerMask.GetMask(new string[] { "Entities", "Default" })))
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
                drawGizmos = true;
                raycastStart = Camera.main.transform.position;
                raycastEnd = hit.point;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(raycastStart, raycastEnd);
        Gizmos.DrawSphere(raycastEnd, 0.2f);
    }
}
