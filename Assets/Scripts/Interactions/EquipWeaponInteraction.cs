using UnityEngine;

public class EquipWeaponInteraction : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float interactionRadius = 0.5f;

    private IWeapon self;

    private void Start()
    {
        self = GetComponent<IWeapon>();
    }

    public float GetInteractionRadius()
    {
        return interactionRadius;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Interact(IEntity interactor)
    {
        //unequip entity's current weapon
        var weaponHolder = interactor.GetWeaponHolder();
        var weapon = weaponHolder.GetComponentInChildren<IWeapon>();
        if (weapon != null)
        {
            _dropWeapon(weapon);
        }

        //zip to the entity's hand
        //TODO

        //equip self to entity
        transform.SetParent(weaponHolder);
        self.DisableInteractions();
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }

    private void _dropWeapon(IWeapon weapon)
    {
        var weaponTransform = weapon.GetTransform();
        weaponTransform.parent = null; //deparent
        weapon.EnableInteractions();
    }
}