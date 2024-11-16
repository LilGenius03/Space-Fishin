using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interact_prompt;

    public virtual void Interact()
    {
        Debug.Log("Player interacted with [" + name + "]");
    }
}
