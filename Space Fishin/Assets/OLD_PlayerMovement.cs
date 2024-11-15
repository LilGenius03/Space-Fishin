/*using UnityEngine;
using UnityEngine.InputSystem.XR;

public class OLD_PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform planet;
    [SerializeField] Camera cam;
    Rigidbody rb;

    #region RotationVars
    [SerializeField]
    private float lookSensitivityX = 1.8f;
    [SerializeField]
    private float lookSensitivityY = 1.8f;
    [SerializeField]
    private float rotationLimit;
    private float currentCamRotationX = 0f;
    #endregion

    public float fake_gravity = 9.8f;

    [Header("Jumping")]
    public float jump_height;
    public float jump_gravity_scale;
    public float checkOffset = 0.1f;
    public bool is_grounded;
    public LayerMask groundLayers;

    bool isCrouched;
    bool isJumping;

    float standColiderHeight = 2f;
    Vector3 standColideroffset = new(0f, 1f, 0f);
    Vector3 crouchColideroffset = new(0f, 0.65f, 0f);
    float crouchColiderHeight = 1.3f;
    Vector3 checkBoxSize = new(0.5f, 0.1f, 0.5f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        int numCollisions = Physics.OverlapBox(transform.position - Vector3.up * checkOffset, checkBoxSize, Quaternion.Euler(Vector3.zero), groundLayers).Length;
        is_grounded = numCollisions > 0;
        if (is_grounded && isJumping)
        {
            isJumping = false;
        }

        // Calculate Planet Gravity (where to suck player to)
        Vector3 down = (planet.transform.position - transform.position).normalized;
        Vector3 forward = Vector3.Cross(transform.right, down);
        transform.rotation = Quaternion.LookRotation(-forward, -down);
        Rotating();

        if (is_grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // Add Planet Gravity (orientates player to sphere)
        rb.AddForce(-transform.up * fake_gravity, ForceMode.Acceleration);

        float forward_input = Input.GetAxis("Vertical");
        float horizontal_input = Input.GetAxis("Horizontal");
        rb.MovePosition(rb.position + (transform.forward * forward_input * 5 * Time.fixedDeltaTime) + (transform.right * horizontal_input * 5 * Time.fixedDeltaTime));
    }

    private void Rotating()
    {
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensitivityX;

        float xRot = Input.GetAxisRaw("Mouse Y");
        float camRotationX = xRot * lookSensitivityY;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCamRotationX -= camRotationX;
        currentCamRotationX = Mathf.Clamp(currentCamRotationX, -rotationLimit, rotationLimit);
        cam.transform.localEulerAngles = new Vector3(currentCamRotationX, 0f, 0f);
    }

    void Jump()
    {
        float jumpForce = Mathf.Sqrt((jump_height * -2) * (-fake_gravity * jump_gravity_scale));
        Debug.Log(jumpForce);
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        isJumping = true;
    }
}*/