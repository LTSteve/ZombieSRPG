using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    [SerializeField]
    private bool maintainOrientation = true;

    private Transform mainCam;

    private void Start()
    {
        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(mainCam, Vector3.up);

        if (maintainOrientation)
        {
            transform.rotation = Quaternion.Euler(Vector3.up * transform.rotation.eulerAngles.y);
        }
    }
}
