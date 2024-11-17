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

    public MeshRenderer[] glowbits;

    Rigidbody rb;

    int pulls = 3;

    [SerializeField] ConstantForce constant_force;
    [SerializeField] ItemList imsd;

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

        if (caught_fish && fishMovement != null)
        {
            bob.transform.position = fishMovement.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if(caught_fish && fishMovement != null)
        {
            //rb.linearVelocity += fishMovement.rb.linearVelocity * Time.fixedDeltaTime;
            constant_force.force = (fishMovement.transform.position - transform.position);
            //constant_force.force = fishMovement.rb.linearVelocity;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(fishMovement != null && other.transform == fishMovement.transform)
        {
            Release();
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
                    fishMovement.Caught(fishMovement.transform.position);
                    caught_fish = true;
                    constant_force.enabled = true;
                    SetGlowbits();
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
            if(caught_fish)
            {
                float chance = (100 - fishMovement.stamina) / 100;
                float ranChance = Random.Range(0, 1);
                if(ranChance > chance)
                {
                    Release(fishMovement);
                    return;
                }


                Vector3 dir = fishMovement.transform.position - transform.position;
                fishMovement.rb.AddForce(-dir * 1000 * fishMovement.resistance, ForceMode.Acceleration);
            }
            else
            {
                StopCoroutine(castCoroutine);
                castCoroutine = StartCoroutine(ReturnCast());
                isCast = false;
            }
            
        }
    }

    public void Release(FishMovement fm = null)
    {
        if (fm != null && fm != fishMovement)
                return;
        caught_fish = false;
        if (fishMovement != null)
            fishMovement.isHooked = false;
        fishMovement = null;
        StopCoroutine(castCoroutine);
        castCoroutine = StartCoroutine(ReturnCast());
        isCast = false;
        constant_force.force = Vector3.zero;
        constant_force.enabled = false;
        SetGlowbits();
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

    void SetGlowbits()
    {
        if (fishMovement != null)
        {
            foreach (MeshRenderer mr in glowbits)
            {
                mr.material.SetColor("_EmissionColor", imsd.rarity_colours[fishMovement.rarity_index]);
            }
        }
        else
        {
            foreach (MeshRenderer mr in glowbits)
            {
                mr.material.SetColor("_EmissionColor", Color.white);
            }
        }
        
    }

}
