using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
    // Object references
    [SerializeField] private Transform interactTransform;
    [SerializeField] private TextMeshProUGUI interactText;

    // List
    [SerializeField] private List<string> interactList;
    private int interactIndex;

    private float textSpeed = 0.015f;
    private float waitTime = 5f;

    public bool isTyping = false;

    private void Start()
    {
        // Only play after player interact with objects
        interactText.text = string.Empty;
        interactTransform.gameObject.SetActive(false);
    }

    public IEnumerator GetInteractText(int objectEnumValue)
    {
        // Impossible to spam E key
        if (isTyping)
            yield break;

        // As text plays, other objects are non-interactable
        isTyping = true;

        // Texts are typed out 1-by-1
        interactText.text = string.Empty;
        interactTransform.gameObject.SetActive(true);

        foreach (char c in interactList[objectEnumValue].ToCharArray())
        {
            interactText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(waitTime);
        interactTransform.gameObject.SetActive(false);

        isTyping = false;
    }    
}