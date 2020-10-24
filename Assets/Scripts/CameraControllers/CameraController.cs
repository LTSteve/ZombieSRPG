using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float CameraSpeed = 10f;
    public float CameraAccelerationRate = 10f;
    public float CameraRotateSpeed = 100f;

    private bool rotating = false;
    private float cameraAccelerationState = 0f;
    private Vector3 cameraMovementDirection = Vector3.zero;
    private Quaternion lerpFrom;
    private Quaternion lerpTo;
    private float cameraLerpState = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        _updateMovementDirection();
        _applyMovementTick();
        _handleRotation();
    }

    private void _updateMovementDirection()
    {
        var xMov = Input.GetAxis("Horizontal");
        var yMov = Input.GetAxis("Vertical");

        cameraMovementDirection = (xMov == 0 && yMov == 0) ? 
            ((cameraMovementDirection * 0.5f).magnitude < 0.1f ? Vector3.zero : (cameraMovementDirection * 0.5f)) : 
            (cameraMovementDirection * 0.9f + new Vector3(xMov, 0f, yMov) * 0.1f);
    }

    private void _applyMovementTick()
    {
        if(cameraMovementDirection.magnitude == 0)
        {
            cameraAccelerationState = 0f;
        }
        else
        {
            cameraAccelerationState = Mathf.Clamp01(cameraAccelerationState + Time.deltaTime * CameraAccelerationRate);

            transform.position = transform.position + (Vector3) (transform.localToWorldMatrix * cameraMovementDirection * Time.deltaTime * CameraSpeed);
        }
    }

    private void _handleRotation()
    {
        if (!rotating)
        {
            var rotate = (int)Input.GetAxis("Rotate");

            if(rotate != 0)
            {
                _setRotation(rotate);
            }
        }

        if (rotating)
        {
            _doRotation();
        }
    }

    private void _setRotation(int rotate)
    {
        if (rotate == 0) return;

        rotating = true;
        cameraLerpState = 0f;
        lerpFrom = transform.rotation;
        lerpTo = transform.rotation * Quaternion.Euler(0f, 45f * rotate, 0f);
    }

    private void _doRotation()
    {
        cameraLerpState = Mathf.Clamp01(cameraLerpState + Time.deltaTime * CameraRotateSpeed);

        transform.rotation = Quaternion.Lerp(lerpFrom, lerpTo, cameraLerpState);

        if(cameraLerpState == 1f)
        {
            rotating = false;
            cameraLerpState = 0f;
        }
    }
}
