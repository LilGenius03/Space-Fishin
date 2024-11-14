using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interaction_dist;
    [SerializeField] LayerMask interaction_layers;
    [SerializeField] Transform cam;


    public void Interact()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, interaction_dist, interaction_layers))
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                hit.transform.GetComponent<Interactable>().Interact();
            }
        }
    }
}
