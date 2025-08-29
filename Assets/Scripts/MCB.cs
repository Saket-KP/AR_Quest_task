using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TriggerInteractable : MonoBehaviour
{
    private XRSimpleInteractable interactable;
    public GameObject lsource;

    void Start()
    {
        // Get the XR Simple Interactable component
        interactable = GetComponent<XRSimpleInteractable>();

        // Subscribe to the select event (trigger press)
        interactable.selectEntered.AddListener(OnTriggerPressed);
    }

    private void OnTriggerPressed(SelectEnterEventArgs args)
    {
        // Logic when the trigger is pressed
        Destroy(lsource);

        Debug.Log("Trigger pressed on object!");

        // Example: Change object color
        GetComponent<Renderer>().material.color = Color.green;

        // Add your custom logic here, e.g., play a sound, trigger an event
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null) audio.Play();
    }

    void OnDestroy()
    {
        // Clean up event listener
        interactable.selectEntered.RemoveListener(OnTriggerPressed);
    }
}