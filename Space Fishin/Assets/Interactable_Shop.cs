using UnityEngine;

public class Interactable_Shop : Interactable
{

    [SerializeField] Shop the_shop;
    public override void Interact()
    {
        base.Interact();
        the_shop.BeginShopping();
    }
}
