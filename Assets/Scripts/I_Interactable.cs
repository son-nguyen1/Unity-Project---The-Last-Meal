using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Interactable
{
    // Know the objects and it's interactable
    string SetObjectName();

    // Press a key to interact with the objects
    void SetObjectInteract();

    // Trigger a sound and new visual
    void SetObjectUpdate();
}