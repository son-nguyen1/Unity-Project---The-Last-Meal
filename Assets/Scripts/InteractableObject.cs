using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum ObjectType
{
    Bar,
    Note,
    CellDoor,
    RoomDoor,
    RoomGuard,
    Beer,
    CrushedCan,
    Cigarette,
    UsedAshtray,
    FriedChicken,
    ChickenBone
}

public class InteractableObject : MonoBehaviour, I_Interactable
{
    [SerializeField] private ObjectType objectType; // Type of interactable object

    // Object reference
    [SerializeField] private Transform inactivePosition;

    // Script reference
    private InteractionManager interactionManager;

    // Dictionaries
    private static readonly Dictionary<ObjectType, string> objectName = new Dictionary<ObjectType, string>
    {
        { ObjectType.Bar, "Look outside" },
        { ObjectType.Note, "Look at Note" },
        { ObjectType.CellDoor, "Open Cell Door" },
        { ObjectType.RoomDoor, "Look outside" },
        { ObjectType.RoomGuard, "Look at Guard" },
        { ObjectType.Beer, "Drink Beer" },
        { ObjectType.CrushedCan, "Crushed Can" },
        { ObjectType.Cigarette, "Smoke Cigarette" },
        { ObjectType.UsedAshtray, "Used Ashtray" },
        { ObjectType.FriedChicken, "Eat Fried Chicken"},        
        { ObjectType.ChickenBone, "Chicken Bone" }
    };

    private static readonly Dictionary<ObjectType, int> objectInteractIndex = new Dictionary<ObjectType, int>
    {
        { ObjectType.Bar, 0 },
        { ObjectType.Note, 1 },
        { ObjectType.CellDoor, 2 },
        { ObjectType.RoomDoor, 3 },
        { ObjectType.RoomGuard, 4 },
        { ObjectType.Beer, 5 },
        { ObjectType.CrushedCan, 6 },
        { ObjectType.Cigarette, 7 },        
        { ObjectType.UsedAshtray, 8 },
        { ObjectType.FriedChicken, 9 },
        { ObjectType.ChickenBone, 10 }
    };

    // Sound effects
    private AudioSource audioSource;
    [SerializeField] private AudioClip beerSound;
    [SerializeField] private AudioClip cigaretteSound;
    [SerializeField] private AudioClip friedChickenSound;

    // Visual effects
    [SerializeField] private GameObject crushedCanPrefab;
    [SerializeField] private GameObject usedAshtrayPrefab;
    [SerializeField] private GameObject chickenBonePrefab;

    // Dictionaries
    public static Dictionary<ObjectType, bool> isSoundPlayed;
    private Dictionary<ObjectType, (AudioClip objectSound, GameObject objectNewPrefab)> objectNewUpdate;

    private void Start()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
        audioSource = GetComponent<AudioSource>();

        // Initialize the object condition
        isSoundPlayed = new Dictionary<ObjectType, bool>
        {
            { ObjectType.Beer, false },
            { ObjectType.Cigarette, false },
            { ObjectType.FriedChicken, false }
        };

        // Initialize the sound and visual
        objectNewUpdate = new Dictionary<ObjectType, (AudioClip, GameObject)>
        {
            { ObjectType.Beer, (beerSound, crushedCanPrefab) },
            { ObjectType.Cigarette, (cigaretteSound, usedAshtrayPrefab) },
            { ObjectType.FriedChicken, (friedChickenSound, chickenBonePrefab) },
        };
    }

    public string SetObjectName()
    {
        // Set a name based on object type, as assigned
        return objectName.TryGetValue(objectType, out string name) ? name : "Unknown";
    }

    public void SetObjectInteract()
    {
        // Trigger an interaction based on object type, as assigned
        if (objectInteractIndex.TryGetValue(objectType, out int index))
        {
            StartCoroutine(interactionManager.GetInteractText(index));
        }
    }

    public void SetObjectUpdate()
    {
        // Trigger a sound and new visual based on object type, as assigned
        if (!interactionManager.isTyping
            && objectNewUpdate.TryGetValue(objectType, out (AudioClip newSound, GameObject newPrefab) objectUpdate))
        {
            if (!isSoundPlayed[objectType]) // Only when condition of object type is false
            {
                UpdateObject(objectUpdate.newSound, objectUpdate.newPrefab);
                isSoundPlayed[objectType] = true;

                // Objects are out-of-reach
                transform.position = inactivePosition.transform.position;
            }
        }
    }

    private void UpdateObject(AudioClip sound, GameObject prefab)
    {
        // Play a sound during the interaction
        audioSource?.PlayOneShot(sound);

        // Show a new visual during the interaction
        if (prefab != null)
        {
            Instantiate(prefab);
        }
    }
}