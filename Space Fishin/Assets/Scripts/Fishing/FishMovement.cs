using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 5f;
    float rotationSpeed = 4.0f;
    public bool isHooked = false;
    public PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((isHooked == true) && (playerController.inputActive == false))
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
        }
    }
}
