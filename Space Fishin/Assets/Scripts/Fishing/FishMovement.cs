using System.Transactions;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 5f;
    float rotationSpeed = 4.0f;
    public bool isHooked = false;
    public PlayerController playerController;
    public Item_Fish itemFish;
    public Rigidbody rb;

    public FishSpawner fishSpawner;
    private bool hasTarget = false;
    private bool isTurning;

    private Vector3 wayPoint;
    private Vector3 lastWayPoint = new Vector3(0f, 0f, 0f);

    //private Animator animator;

    private Collider col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fishSpawner = transform.parent.GetComponentInParent<FishSpawner>();
        playerController = FindAnyObjectByType<PlayerController>();
        //animator = GetComponent<Animator>();
        SetUpNPC();
    }

    void SetUpNPC()
    {
        //float scale = Random.Range(0f, 4f);
        //transform.localScale += new Vector3(scale * 1.5f, scale, scale);
        if(transform.GetComponent<Collider>() != null && transform.GetComponent<Collider>().enabled == true)
        {
            col = transform.GetComponent<Collider>();
        }
        else if(transform.GetComponentInChildren<Collider>() != null && transform.GetComponentInChildren<Collider>().enabled == true)
        {
            col = transform.GetComponentInChildren<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHooked == false)
        {
            if (!hasTarget)
            {
                hasTarget = CanFindTarget();
            }
            else
            {
                RotateNPC(wayPoint, speed);
                transform.position = Vector3.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);

                CollidedNPC();
            }

            if (transform.position == wayPoint)
            {
                hasTarget = false;
            }
        }
        else if ((isHooked == true) && (playerController.inputActive == false))
        {
            playerController.bob.transform.position = transform.position;
            Vector3 direction = transform.forward;
            transform.Translate(0, 0, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
        else if((isHooked == true) && (playerController.inputActive == true))
        {
            playerController.bob.transform.position = transform.position;
            transform.position = Vector3.Lerp(transform.position, playerController.bobArea.transform.position, playerController.reelSpeed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isHooked = false;
            playerController.ReturnCast();
            this.gameObject.SetActive(false);
            Inventory.instance.AddItem(itemFish);
        }
    }

    void CollidedNPC()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, transform.localScale.z))
        {
            if(hit.collider == col || hit.collider.CompareTag("Waypoint"))
            {
                return;
            }
            int randomNum = Random.Range(1, 100);
            if (randomNum < 40)
                hasTarget = false;
            //Debug.Log(hit.collider.transform.parent.name + " " + hit.collider.transform.parent.position);
        }
    }

    bool CanFindTarget(float start = 1f, float end = 7f)
    {
        wayPoint = fishSpawner.RandomWaypoint();
        if(lastWayPoint == wayPoint)
        {
            wayPoint = fishSpawner.RandomWaypoint();
            return false;
        }
        else
        {
            lastWayPoint = wayPoint;
            speed = Random.Range(start, end);
            //animator.speed = speed;
            return true;
        }
    }

    void RotateNPC(Vector3 wayPoint, float currentSpeed)
    {
        float TurnSpeed = currentSpeed * Random.Range(1f, 3f);
        Vector3 LookAt = wayPoint - this.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookAt), TurnSpeed * Time.deltaTime);
    }
}
