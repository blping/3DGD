using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement; // Assign Player GameObject here

    [Header("Settings")]
    public float lookSensitivity = 500f;
    public float verticalClamp = 60f;

    private float xRotation = 0; 

    void Update()
    {
        Debug.Log("Hello, follow target update works!");
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        // Vertical Rotation (Always isolated to the Neck/Camera Target)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (playerMovement.isThirdPerson)
        {
            Debug.Log("Hello, third person works!");
            // THIRD-PERSON: Horizontal input orbits the camera by rotating the Player's parent.
            // The PlayerMovement script handles the Player's orientation when moving.
            transform.parent.Rotate(Vector3.up * mouseX);
        }
        
        else // isThirdPerson == false (FIRST-PERSON VIEW)
        {
            Debug.Log("Hello, first person works!");
            // Use an even higher multiplier here, as it's often slower
            mouseX = Input.GetAxis("Mouse X") * 800f * Time.deltaTime; 
            transform.rotation *= Quaternion.Euler(0, mouseX, 0);

            // The vertical look (mouseY) should be in CameraFollowTarget.cs
        }

        // FIRST-PERSON: The PlayerMovement script handles the horizontal rotation of the Player body.
        // We do NOTHING here for horizontal rotation, keeping it clean.
    }
}