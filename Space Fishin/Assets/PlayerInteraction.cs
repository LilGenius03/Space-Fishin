using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interaction_dist;
    [SerializeField] LayerMask interaction_layers;
    [SerializeField] Transform cam;
    [SerializeField] TextMeshProUGUI interaction_prompt;

    private void Start()
    {
        StartCoroutine(CheckInteractions());
    }

    IEnumerator CheckInteractions()
    {
        bool y = true;
        RaycastHit hit;
        while (y == true)
        {
            if (Physics.Raycast(cam.position, cam.forward, out hit, interaction_dist, interaction_layers))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Interactable"))
                {
                    interaction_prompt.text = hit.transform.GetComponent<Interactable>().interact_prompt;
                }
                else
                    interaction_prompt.text = "";
            }
            else
                interaction_prompt.text = "";
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void Interact()
    {        
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, interaction_dist, interaction_layers))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Interactable"))
            {
                hit.transform.GetComponent<Interactable>().Interact();
            }
        }
    }
}
