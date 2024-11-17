using System.Linq;
using System.Transactions;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public int rarity_index; // 0 -common 2-legendary

    public float speed = 5f;
    float rotationSpeed = 4.0f;
    public float resistance = 1;
    public bool isHooked = false;
    public PlayerFishing playerController;
    public Item_Fish itemFish;
    public Rigidbody rb;

    public FishSpawner fishSpawner;
    private bool hasTarget = false;
    private bool isTurning;

    private Vector3 wayPoint;
    private Vector3 lastWayPoint = new Vector3(0f, 0f, 0f);

    [SerializeField] float max_speed, min_speed;

    [SerializeField] float max_runaway_distance;
    Vector3 runa_waypoint;
    bool has_runaway_point;
    float panic_mult;
    [SerializeField] float max_panic_mult;

    //public 

    //private Animator animator;

    private Collider col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fishSpawner = transform.parent.GetComponentInParent<FishSpawner>();
        playerController = FindAnyObjectByType<PlayerFishing>();
        //animator = GetComponent<Animator>();
        SetUpNPC();
        rb.constraints = RigidbodyConstraints.FreezeAll;
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
        else
        {
            Vector3 dir = runa_waypoint - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), speed * panic_mult * Time.deltaTime);
           // transform.position = Vector3.MoveTowards(transform.position, runa_waypoint, speed * panic_mult * Time.deltaTime);
            if (Vector3.Distance(transform.position, runa_waypoint) < 0.5)
                GetRunawayPoint();
        }
    }

    private void FixedUpdate()
    {
        if (isHooked)
        {
            Vector3 dir = runa_waypoint - transform.position;
            rb.linearVelocity = dir * Time.fixedDeltaTime * speed * panic_mult;
            //rb.MovePosition(transform.position + (dir * speed * panic_mult * Time.fixedDeltaTime));
            Debug.Log(rb.linearVelocity);
            //Quaternion.LookRotation(dir);
        }
    }

    public void Caught(Vector3 pos)
    {
        if (!isHooked)
        {
            isHooked = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            GetRunawayPoint();
            panic_mult = Random.Range(1, max_panic_mult);
        }
    }

    void GetRunawayPoint()
    {
        runa_waypoint = transform.position + new Vector3(Random.Range(-max_runaway_distance, max_runaway_distance), Random.Range(-max_runaway_distance, max_runaway_distance), Random.Range(-max_runaway_distance, max_runaway_distance));
        Debug.Log(Vector3.Distance(transform.position, Vector3.zero));
        while(Vector3.Distance(runa_waypoint, Vector3.zero) < 40)
          runa_waypoint = transform.position + new Vector3(Random.Range(-max_runaway_distance, max_runaway_distance), Random.Range(-max_runaway_distance, max_runaway_distance), Random.Range(-max_runaway_distance, max_runaway_distance));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isHooked)
        {
            isHooked = false;
            playerController.Release(this);
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

    bool CanFindTarget(float start = -4f, float end = 4f)
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
            speed = Random.Range(min_speed, max_speed);
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
