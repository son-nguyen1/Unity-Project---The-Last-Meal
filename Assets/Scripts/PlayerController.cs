using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Object references
    [SerializeField] private Camera m_Camera;
    [SerializeField] private Transform objectPrompt;
    [SerializeField] private TextMeshProUGUI objectName;

    [SerializeField] private Transform bangPrompt;
    [SerializeField] private TextMeshProUGUI bangText;

    [SerializeField] private Transform endPrompt;
    [SerializeField] private TextMeshProUGUI endText;

    // Script reference
    private InteractionManager interactionManager;
    private GuardController guardController;

    // Sound effects
    private AudioSource audioSource;
    [SerializeField] private AudioClip doorBangSound;

    [SerializeField] private float moveSpeed = 1.5f;

    private bool isWalking = false;
    private bool hasBangedDoor = false;

    private void Start()
    {
        // Detect only objects with these scripts
        interactionManager = FindObjectOfType<InteractionManager>();
        guardController = FindObjectOfType<GuardController>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleInteraction();
        CallGroundGuard();
        ReadyForEnding();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private bool HandleDetection(out RaycastHit hit, float detectDistance)
    {
        // Detect objects at close proximity
        Vector3 centerOfScreen = (Vector3.one / 2f);
        Ray ray = m_Camera.ViewportPointToRay(centerOfScreen);

        return Physics.Raycast(ray, out hit, detectDistance);
    }

    public void HandleInteraction()
    {
        // Retrieve data from detected objects
        const float interactDistance = 0.75f;
        if (HandleDetection(out RaycastHit hit, interactDistance))
        {
            // Only object with these scripts
            if (hit.collider.TryGetComponent(out I_Interactable interactable)) // Interface
            {
                objectName.text = interactable.SetObjectName();
                objectPrompt.gameObject.SetActive(!interactionManager.isTyping);

                if (Input.GetKeyDown(KeyCode.E) && !interactionManager.isTyping)
                {
                    interactable.SetObjectUpdate();
                    interactable.SetObjectInteract();
                }
            }
            if (hit.collider.TryGetComponent(out CellDoor cellDoor))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(cellDoor.OpenCellDoor());
                }
            }
        }
        else
        {
            // Only appear as objects are detected
            objectPrompt.gameObject.SetActive(false);
        }    
    }

    private void CallGroundGuard()
    {
        // Retrieve data from detected objects
        const float interactDistance = 0.75f;
        if (HandleDetection(out RaycastHit hit, interactDistance))
        {
            // Only object with this script
            if (hit.collider.TryGetComponent(out RoomDoor roomDoor))
            {
                bangText.text = "Call the Guard";
                bangPrompt.gameObject.SetActive(!interactionManager.isTyping);

                if (Input.GetKeyDown(KeyCode.F) && !hasBangedDoor && !interactionManager.isTyping)
                {   
                    hasBangedDoor = true;
                    audioSource.PlayOneShot(doorBangSound);
                    guardController.DoorIsBanging();
                }
            }
        }
        else
        {
            // Only appear as objects are detected
            bangPrompt.gameObject.SetActive(false);
        }      
    }

    private void ReadyForEnding()
    {
        // Retrieve data from detected objects
        const float interactDistance = 0.75f;
        if (HandleDetection(out RaycastHit hit, interactDistance))
        {
            // Only object with this script
            if (hit.collider.TryGetComponent(out GuardController guardController))
            {
                endText.text = "I'm Ready";
                endPrompt.gameObject.SetActive(!interactionManager.isTyping);

                if (Input.GetKeyDown(KeyCode.E) && !interactionManager.isTyping)
                {
                    SceneManager.LoadScene(3);
                }
            }
        }
        else
        {
            // Only appear as objects are detected
            endPrompt.gameObject.SetActive(false);
        }     
    }

    private void HandleMovement()
    {
        // Establish key inputs on axis
        Vector3 inputVector = new Vector3(0f, 0f, 0f);

        if (Input.GetKey(KeyCode.W))    { inputVector.z += 1; }
        if (Input.GetKey(KeyCode.S))    { inputVector.z -= 1; }
        if (Input.GetKey(KeyCode.D))    { inputVector.x += 1; }
        if (Input.GetKey(KeyCode.A))    { inputVector.x -= 1; }

        inputVector = inputVector.normalized;
        
        // Apply inputs to move the player
        Vector3 moveDir = transform.forward * inputVector.z + transform.right * inputVector.x;
        moveDir.y = 0f; // Prevent player from flying

        // Detect and slide along obstacles
        float bcDistance = 0.1f;
        Vector3 bcHalfExtents = new Vector3(0.25f, 1f, 0.25f);

        bool isCollided = Physics.BoxCast(transform.position, bcHalfExtents, moveDir, Quaternion.identity, bcDistance);

        if (isCollided)
        {
            if (Physics.BoxCast(transform.position, bcHalfExtents, Vector3.forward, out RaycastHit forwardHit, Quaternion.identity, bcDistance))
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, forwardHit.normal);
            }

            if (Physics.BoxCast(transform.position, bcHalfExtents, Vector3.back, out RaycastHit backHit, Quaternion.identity, bcDistance))
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, backHit.normal);
            }

            if (Physics.BoxCast(transform.position, bcHalfExtents, Vector3.right, out RaycastHit rightHit, Quaternion.identity, bcDistance))
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, rightHit.normal);
            }

            if (Physics.BoxCast(transform.position, bcHalfExtents, Vector3.left, out RaycastHit leftHit, Quaternion.identity, bcDistance))
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, leftHit.normal);
            }
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;
        isWalking = (moveDir != Vector3.zero); // Walk animation
    }

    public bool PlayerIsWalking()
    {
        return isWalking;
    }
}