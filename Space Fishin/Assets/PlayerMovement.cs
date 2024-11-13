using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate Planet Gravity (where to suck player to)
        Vector3 down = (planet.transform.position - transform.position).normalized;
        Vector3 forward = Vector3.Cross(transform.right, down);
        transform.rotation = Quaternion.LookRotation(-forward, -down);
    }

    private void FixedUpdate()
    {
        // Add Planet Gravity (orientates player to sphere)
        rb.AddForce(-transform.up * 9.8f, ForceMode.Acceleration);

        float forward_input = Input.GetAxis("Vertical");
        float horizontal_input = Input.GetAxis("Horizontal");
        rb.MovePosition(rb.position + (transform.forward * forward_input * 2 * Time.fixedDeltaTime) + (transform.right * horizontal_input * 2 * Time.fixedDeltaTime));
        Rotating();
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
}