using System.Collections;
using UnityEngine;

public class Interactable_Chair : Interactable
{
    [SerializeField] GameObject get_up;
    [SerializeField] Transform MoveToPos;
    bool is_sitting;
    Coroutine sit_coroutine;

    public override void Interact()
    {
        base.Interact();
        if (!is_sitting)
        {
            PlayerManager.instance.player_movement.ForceIntoPosition(MoveToPos);
            sit_coroutine = StartCoroutine(HackFix());
            is_sitting = true;
            //interact_prompt = "get up?";
        }
       

    }

    private void Update()
    {
        if(is_sitting && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            PlayerManager.instance.player_movement.FreePlayerFromPosition();
            is_sitting = false;
        }

    }

    IEnumerator HackFix()
    {
        yield return null;
        //get_up.SetActive(true);
        PlayerManager.instance.player_movement.ForceIntoPosition(MoveToPos);
    }

}
