using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("References")]
    // NEW: Reference to the PlayerMovement script to control player rotation behavior
    public PlayerMovement playerMovement;

    [Header("Cameras")]
    public CinemachineVirtualCamera thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.V;
    public int highPriority = 15; // Highest priority wins
    public int lowPriority = 5;

    private bool isThirdPerson = true;

    void Start()
    {
        // Start in 3rd person view
        SetThirdPersonView();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (isThirdPerson)
            {
                SetFirstPersonView();
            }
            else
            {
                SetThirdPersonView();
            }
        }
    }

    private void SetThirdPersonView()
    {
        // 1. Swap VCam Priorities (Original Logic)
        thirdPersonCam.Priority = highPriority;
        firstPersonCam.Priority = lowPriority;
        isThirdPerson = true;

        // 2. NEW: Update PlayerMovement Script
        if (playerMovement != null)
        {
            playerMovement.isThirdPerson = true;
        }

        // 3. Lock Cursor (Original Logic)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        /*
        // Third-person camera takes control
        thirdPersonCam.Priority = highPriority;
        firstPersonCam.Priority = lowPriority;
        isThirdPerson = true;

        // Unlock cursor for the typical free-orbiting third-person control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        */
    }

    private void SetFirstPersonView()
    {

        // 1. Swap VCam Priorities (Original Logic)
        firstPersonCam.Priority = highPriority;
        thirdPersonCam.Priority = lowPriority;
        isThirdPerson = false;

        // 2. NEW: Update PlayerMovement Script
        if (playerMovement != null)
        {
            playerMovement.isThirdPerson = false;
        }

        // 3. Lock Cursor (Original Logic)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        /*
        // First-person camera takes control
        firstPersonCam.Priority = highPriority;
        thirdPersonCam.Priority = lowPriority;
        isThirdPerson = false;

        // Keep cursor locked for FPS-style control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        */
    }
}