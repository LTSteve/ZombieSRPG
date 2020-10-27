using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class AbstractWeapon : MonoBehaviour, IWeapon
{
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;
    private Rigidbody body;

    protected void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        body = GetComponent<Rigidbody>();
    }

    public void DisableInteractions()
    {
        boxCollider.enabled = false;
        sphereCollider.enabled = false;
        body.isKinematic = true;
        body.useGravity = false;
        body.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void EnableInteractions()
    {
        boxCollider.enabled = true;
        sphereCollider.enabled = true;
        body.isKinematic = false;
        body.useGravity = true;
        body.constraints = 0;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}