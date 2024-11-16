using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFishing : MonoBehaviour
{
    public FishingRod fishingRod;
    FishMovement fishMovement;

    public Camera cam;
    public GameObject bob;
    Coroutine castCoroutine;
    public bool isCast;
    public bool bobActive = false;
    public GameObject bobArea;

    bool caught_fish;

    public float lineDistance;
    public float lineSpeed;
    public float bobSpeed;
    [SerializeField] LayerMask fishingLayer;
    public float reelSpeed;
    public bool inputActive;

    [Header("Baits")]
    Inventory inv;
    public Item_Bait current_bait;
    int current_bait_index;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        bob.transform.rotation = transform.rotation;
        if (!isCast && !bobActive)
        {
            Vector3 LerpedPosition = Vector3.Lerp(bob.transform.position, bobArea.transform.position, Time.fixedDeltaTime * bobSpeed);
            bob.transform.position = new Vector3(LerpedPosition.x, bobArea.transform.position.y, LerpedPosition.z);
        }
    }

    private void FixedUpdate()
    {
        if(caught_fish && fishMovement != null)
        {
            rb.linearVelocity += fishMovement.rb.linearVelocity;
        }
    }

    public void CastRod()
    {
        inputActive = true;
        if (isCast == false)
        {
            isCast = true;
            bobActive = true;
            if (castCoroutine != null)
            {
                StopCoroutine(castCoroutine);
            }
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, lineDistance, fishingLayer))
            {
                //bob.transform.position = hit.point;
                castCoroutine = StartCoroutine(CastTravel(hit.point));
                if (hit.transform.CompareTag("FishingArea"))
                {
                    fishMovement = hit.transform.GetComponent<FishMovement>();
                    fishMovement.Caught(hit.point);
                    caught_fish = true;
                }
            }
            else
            {
                //bob.transform.position = cam.transform.position + (cam.transform.forward * lineDistance);
                castCoroutine = StartCoroutine(CastTravel(cam.transform.position + (cam.transform.forward * lineDistance)));
            }
        }
        else
        {
            StopCoroutine(castCoroutine);
            castCoroutine = StartCoroutine(ReturnCast());
            isCast = false;
        }
    }

    public IEnumerator CastTravel(Vector3 castPosition)
    {
        while (Vector3.Distance(bob.transform.position, castPosition) > 0)
        {
            //bob.transform.position += cam.transform.forward * Time.deltaTime;
            bob.transform.position = Vector3.Lerp(bob.transform.position, castPosition, (lineSpeed * Time.deltaTime));
            yield return null;
        }
        bob.transform.position = castPosition;
    }

    public IEnumerator ReturnCast()
    {
        while (Vector3.Distance(bob.transform.position, bobArea.transform.position) > 1)
        {
            //bob.transform.position += -cam.transform.forward * Time.deltaTime;
            bob.transform.position = Vector3.Lerp(bob.transform.position, bobArea.transform.position, (lineSpeed * Time.deltaTime));
            yield return null;
        }
        bob.transform.position = bobArea.transform.position;
        bobActive = false;
    }

    public void SwitchBait(bool up)
    {
        if (inv.baits.Count == 0)
        {

        }
        else
        {
            if (up)
            {
                current_bait_index++;
                if (current_bait_index >= inv.baits.Count)
                    current_bait_index = 0;
                current_bait = inv.baits[current_bait_index];
            }
            else
            {
                current_bait_index--;
                if (current_bait_index < 0)
                    current_bait_index = inv.baits.Count - 1;
                current_bait = inv.baits[current_bait_index];
            }
        }

    }

}
