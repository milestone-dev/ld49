using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InteractionTriggerType
{
    Click,
    Collide
}

public class InteractableObject : MonoBehaviour
{
    public InteractionTriggerType triggerType;
    public UnityEvent onInteract;
    public bool interactionEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        if (triggerType == InteractionTriggerType.Collide)
            GetComponent<Collider>().isTrigger = true;
    }

    public void Interact()
    {
        if (!interactionEnabled)
            return;

        if (this.onInteract == null)
        {
            Debug.LogError("Missing onInteract target");
            return;
        }

        this.onInteract.Invoke();
    }

    public void Collide()
    {
            
    }
}
