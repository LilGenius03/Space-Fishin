using UnityEngine;

public class Interactable_FuelStation : Interactable
{
    public override void Interact()
    {
        base.Interact();
        PlayerManager.instance.player_movement.FillUpFuel();
    }
}
