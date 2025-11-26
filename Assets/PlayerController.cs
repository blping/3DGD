using UnityEngine;

public class playerController : MonoBehaviour
{
    public Transform cameraTransform;   
    public CharacterController controller; 

    
    [Header("Settings")]
    public float moveSpeed = 5;
    public float lookSensitivity = 500; 
    public float switchSpeed = 8;

    [Header("View")]
    public KeyCode toggleKey = KeyCode.C;
    public Vector3 thirdPersonOffset = new Vector3(0, 3, -5);
    public Vector3 firstPersonOffset = new Vector3(0, 0, 0);

    private bool isFirstPerson = false;
    private float xRotation = 0; //store rotation(up down) of camera
    private Vector3 targetLocalPosition;

    void Start()
    {
        targetLocalPosition = thirdPersonOffset;
        cameraTransform.localPosition = targetLocalPosition;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        movement();
        viewToggle();
        lookRotation();
        cameraOffset();
    }

    private void movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    private void viewToggle()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isFirstPerson = !isFirstPerson;

            if (isFirstPerson)
            {
                targetLocalPosition = firstPersonOffset;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                targetLocalPosition = thirdPersonOffset;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void lookRotation()
    {
        bool isLooking = isFirstPerson || Input.GetMouseButton(1); //rightclick

        if (isLooking)
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

            //horizontal
            transform.Rotate(Vector3.up * mouseX);

            //vertical
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90); //prevent camera from flipping over
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0); //Quaternion.Euler: use x, y, z degrees

            //splits horizontal and vertical rotation so the model wont be affected
        }
        else if (!isFirstPerson)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void cameraOffset()
    {
        //smoothly move camera
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetLocalPosition, Time.deltaTime * switchSpeed);
    }
}