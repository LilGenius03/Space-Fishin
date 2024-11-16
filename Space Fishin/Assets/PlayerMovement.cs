using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    [SerializeField] Transform planet;
    [SerializeField] Transform cam_pivot;
    [SerializeField] CapsuleCollider col;
    PlayerController player_controller;
    public GameObject fishrod_graphics;
    public GameObject line_graphics;

    float current_speed;
    Vector2 movement_input;
    Vector2 look_input;
    Vector3 velocity;
    Vector3 x_vel;
    Vector3 y_vel;

    [Header("Camera")]
    [SerializeField] float lookSensitivityX = 1.8f;
    [SerializeField] float lookSensitivityY = 1.8f;
    [SerializeField] private float rotationLimit;
    private float currentCamRotationX = 0f;
    bool reverse_roll;
    private float currentCamRotationZ = 0f;
    private float currentCamRotationY = 0f;
    bool is_rolling;
    Vector3 original_cam_pos;

    [Header("Physics")]
    [SerializeField] float fake_gravity = 9.8f;
    bool is_affected_by_gravity = true;
    [SerializeField] float drag;
    [SerializeField] float orientate_speed;

    [Header("Walking")]
    [SerializeField] float speed_walk;
    [SerializeField] float speed_crouch;

    [Header("Jumping")]
    [SerializeField] float jump_height;
    [SerializeField] float jump_gravity_scale;
    bool is_grounded;
    public LayerMask ground_layers;

    [Header("Crouching")]
    [SerializeField] float height_stand;
    [SerializeField] float height_crouch;
    bool is_crouching;

    [Header("Thrusters")]
    [SerializeField] float thruster_force = 100f;
    [SerializeField] float thruster_force_grounded = 500f;
    public float fuel;
    [SerializeField] float fuel_max;
    bool out_of_fuel;
    bool is_ascending;
    bool is_descending;
    [SerializeField] Slider fuel_ui;
    [SerializeField] GameObject outoffuel_ui;

    float oxygen;
    [SerializeField] float max_oxygen = 30;
    [SerializeField] float oxygen_return_rate = 2;
    [SerializeField] Slider oxygen_ui;
    [SerializeField] GameObject lowoxygen_ui;
    public bool is_dead;

    [Header("Ground Checking")]
    public float check_offset = 0.1f;
    Vector3 ground_check_box = new(0.5f, 0.1f, 0.5f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player_controller = GetComponent<PlayerController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_speed = speed_walk;
        original_cam_pos = cam_pivot.localPosition;
        fuel_ui.maxValue = fuel_max;
        oxygen_ui.maxValue = max_oxygen;
        oxygen = max_oxygen;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_affected_by_gravity)
        {
            //NormalLook();
            CheckGrounded();
            OrientateToPlanet();
            if (oxygen < max_oxygen)
                oxygen += oxygen_return_rate * Time.deltaTime;
            else
                oxygen = max_oxygen;
        }
        else
        {
            oxygen -= Time.deltaTime;
            SpaceLook();
        }
        fuel_ui.value = fuel;
        if(fuel <= 0 && !out_of_fuel)
        {
            fuel = 0;
            out_of_fuel = true;
            outoffuel_ui.SetActive(true);
        }
        oxygen_ui.value = oxygen;
        if (oxygen <= 0 && !is_dead)
        {
            oxygen = 0;
            is_dead = true;
            //Dead Func;
        }
    }

    void FixedUpdate()
    {
        if (is_affected_by_gravity)
        {
            ApplyGravity();
            Walk();
        }
        else
        {
            if(fuel > 0)
                ThrusterMove();
        }

        Ascend();
        Descend();
    }

    private void LateUpdate()
    {
        if (is_affected_by_gravity)
        {
            NormalLook();
        }
    }

    void CheckGrounded()
    {
        int numCollisions = Physics.OverlapBox(transform.position - Vector3.up * check_offset, ground_check_box, Quaternion.Euler(Vector3.zero), ground_layers).Length;
        is_grounded = numCollisions > 0;
    }

    void OrientateToPlanet()
    {
        Vector3 down = (planet.position - transform.position).normalized;
        Vector3 forward = Vector3.Cross(transform.right, down);
        transform.rotation = Quaternion.LookRotation(-forward, -down);
    }

    void ApplyGravity()
    {
        // Add Planet Gravity (orientates player to sphere)
        /*float grav_mult = Vector3.Distance(transform.position, planet.position) / 40;
        grav_mult = ExtensionMethods.Map(grav_mult, 0, 1, 1, 0);
        grav_mult += 0.4f;
        Debug.Log(grav_mult);*/
        rb.AddForce(-transform.up * fake_gravity /** grav_mult*/, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            is_affected_by_gravity = true;

            float newCamRot = Mathf.Clamp(transform.eulerAngles.x, -rotationLimit, rotationLimit);
            cam_pivot.transform.localEulerAngles = new Vector3(newCamRot, 0f, 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            is_affected_by_gravity = false;

            if (cam_pivot.transform.localEulerAngles.x < 0)
                reverse_roll = true;

            //currentCamRotationY = transform.eulerAngles.y;
            //transform.rotation = Quaternion.Euler(currentCamRotationX, transform.eulerAngles.y, transform.eulerAngles.y);
            //cam_pivot.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    void NormalLook()
    {
        Vector3 y_rot = new Vector3(0f, look_input.x, 0f) * lookSensitivityX;
        transform.rotation = transform.rotation * Quaternion.Euler(y_rot);

        float camRotationX = look_input.y * lookSensitivityY;
        currentCamRotationX -= camRotationX;
        currentCamRotationX = Mathf.Clamp(currentCamRotationX, -rotationLimit, rotationLimit);
        cam_pivot.transform.localEulerAngles = new Vector3(currentCamRotationX, 0f, 0f);
    }


    IEnumerator LerpToOrientate(Quaternion orientate)
    {
        while (transform.rotation != orientate)
        {
            //bob.transform.position += -cam.transform.forward * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, orientate, Time.deltaTime * orientate_speed);
            yield return null;
        }
    }

    #region Neutral Movement
    void Walk()
    {
        velocity = Vector3.zero;

        if (movement_input.x > 0.1f || movement_input.x < -0.1f)
            x_vel = transform.right * (current_speed * movement_input.x * Time.fixedDeltaTime);
        else
            x_vel = Vector3.Lerp(x_vel, Vector3.zero, Time.fixedDeltaTime * drag);

        if (movement_input.y > 0.1f || movement_input.y < -0.1f)
            y_vel = transform.forward * (current_speed * movement_input.y * Time.fixedDeltaTime);
        else
            y_vel = Vector3.Lerp(y_vel, Vector3.zero, Time.fixedDeltaTime * drag);

        velocity = x_vel + y_vel;
        rb.MovePosition(transform.position + velocity);
    }

    public void Jump()
    {
        if (is_grounded)
        {
            UnCrouch();
            float jumpForce = Mathf.Sqrt(jump_height * -2 * (-fake_gravity * jump_gravity_scale));
            Debug.Log(jumpForce);
            rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }
        
    }

    public void DoCrouch()
    {
        if (is_grounded)
        {
            if (is_crouching)
                UnCrouch();
            else
                Crouch();
        }
    }

    public void Crouch()
    {
        is_crouching = true;

        col.height = height_crouch;
        col.center = Vector3.up * (height_crouch / 2);

        cam_pivot.localPosition = Vector3.up * (height_crouch - 0.2f);

        current_speed = speed_crouch;
    }

    public void UnCrouch()
    {
        is_crouching = false;

        col.height = height_stand;
        col.center = Vector3.up * (height_stand / 2);

        cam_pivot.localPosition = Vector3.up * (height_stand - 0.2f);

        current_speed = speed_walk;
    }
    #endregion

    #region Thruster Movement
    void SpaceLook()
    {
        float camRotationX = -look_input.y * lookSensitivityY;
        currentCamRotationX -= camRotationX;
        float otherCamRotation = 0f;
        if(!reverse_roll)
            otherCamRotation = look_input.x * lookSensitivityX;
        else
            otherCamRotation = -look_input.x * lookSensitivityX;

        if (!is_rolling)
            transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(camRotationX, 0f, otherCamRotation));
        else
            transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0f, otherCamRotation, 0f));
    }

    void ThrusterMove()
    {
        rb.AddForce(cam_pivot.forward * thruster_force * movement_input.y * Time.fixedDeltaTime, ForceMode.Acceleration);
        rb.AddForce(cam_pivot.right * thruster_force * movement_input.x * Time.fixedDeltaTime, ForceMode.Acceleration);
        if(movement_input != Vector2.zero)
        {
            fuel -= Time.fixedDeltaTime;
        }
    }

    void Ascend()
    {
        if (!is_ascending)
            return;

        if(is_affected_by_gravity)
            rb.AddForce(transform.up * thruster_force_grounded * Time.fixedDeltaTime, ForceMode.Acceleration);
        else
            rb.AddForce(cam_pivot.up * thruster_force * Time.fixedDeltaTime, ForceMode.Acceleration);

        fuel -= Time.fixedDeltaTime;
    }

    void Descend()
    {
        if (!is_descending)
            return;

        if (is_affected_by_gravity)
            rb.AddForce(-transform.up * thruster_force_grounded * Time.fixedDeltaTime, ForceMode.Acceleration);
        else
            rb.AddForce(-cam_pivot.up * thruster_force * Time.fixedDeltaTime, ForceMode.Acceleration);

        fuel -= Time.fixedDeltaTime;
    }
    #endregion

    public void FillUpFuel()
    {
        fuel = fuel_max;
    }

    public void ForceIntoPosition(Transform new_pos)
    {
        //StartCoroutine(ForcingMove(new_pos, speed));
        Debug.Log("Moving Player");
        player_controller.FreezeInput();
        cam_pivot.localRotation = Quaternion.identity;
        rb.detectCollisions = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        cam_pivot.transform.position = new_pos.position;
        cam_pivot.transform.rotation = new_pos.rotation;
    }

    public void FreePlayerFromPosition()
    {
        GetComponent<PlayerController>().UnFreezeInput();
        rb.detectCollisions = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if(is_crouching)
            cam_pivot.localPosition = Vector3.up * (height_crouch - 0.2f);
        else
            cam_pivot.localPosition = Vector3.up * (height_stand - 0.2f);
        //Camera.main.fieldOfView = 90;
    }

    public void SetMovementInput(Vector2 _new_input_dir)
    {
        movement_input = _new_input_dir.normalized;
    }

    public void SetLookInput(Vector2 _new_input_dir)
    {
        look_input = _new_input_dir;
    }

    public void SetAscend(bool y)
    {
        is_ascending = y;
    }

    public void SetDescend(bool y)
    {
        is_descending = y;
    }

    public void SetRolling(bool y)
    {
        is_rolling = y;
    }
}
