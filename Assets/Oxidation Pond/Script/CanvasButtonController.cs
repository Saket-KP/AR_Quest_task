using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.SceneManagement;
using System.Collections;

public class CanvasButtonController : MonoBehaviour
{
    [Header("VR Interactable Objects")]
    public XRSimpleInteractable button1Interactable;
    public XRSimpleInteractable button2Interactable;
    
    [Header("UI References")]
    public Canvas targetCanvas;
    
    [Header("Game Objects to Enable")]
    public GameObject gameObject1;
    public GameObject gameObject2;
    
    [Header("Scene Restart Settings")]
    public float timeoutDuration = 10f;
    
    private Coroutine timeoutCoroutine;
    private bool buttonClicked = false;
    
    void Start()
    {
        // Subscribe to VR interaction events for button 1
        if (button1Interactable != null)
        {
            button1Interactable.selectEntered.AddListener((args) => OnVRButtonClicked(gameObject1, button1Interactable));
        }
        
        // Subscribe to VR interaction events for button 2
        if (button2Interactable != null)
        {
            button2Interactable.selectEntered.AddListener((args) => OnVRButtonClicked(gameObject2, button2Interactable));
        }
        
        // If no canvas is assigned, try to get the canvas this script is attached to
        if (targetCanvas == null)
        {
            targetCanvas = GetComponent<Canvas>();
            if (targetCanvas == null)
            {
                targetCanvas = GetComponentInParent<Canvas>();
            }
        }
    }
    
    void OnEnable()
    {
        // Start the timeout timer when the canvas becomes active
        StartTimeoutTimer();
    }
    
    void OnDisable()
    {
        // Stop the timeout timer when the canvas is disabled
        StopTimeoutTimer();
    }
    
    void StartTimeoutTimer()
    {
        // Reset the button clicked flag
        buttonClicked = false;
        
        // Stop any existing timeout timer
        StopTimeoutTimer();
        
        // Start a new timeout timer
        timeoutCoroutine = StartCoroutine(TimeoutCountdown());
        Debug.Log("Started VR timeout timer for " + timeoutDuration + " seconds");
    }
    
    void StopTimeoutTimer()
    {
        if (timeoutCoroutine != null)
        {
            StopCoroutine(timeoutCoroutine);
            timeoutCoroutine = null;
            Debug.Log("Stopped VR timeout timer");
        }
    }
    
    IEnumerator TimeoutCountdown()
    {
        yield return new WaitForSeconds(timeoutDuration);
        
        // If no button was clicked within the timeout duration
        if (!buttonClicked)
        {
            Debug.Log("No VR button interacted within " + timeoutDuration + " seconds. Restarting scene...");
            RestartScene();
        }
    }
    
    void OnVRButtonClicked(GameObject objectToEnable, XRSimpleInteractable buttonInteractable)
    {
        // Mark that a button was clicked
        buttonClicked = true;
        
        // Stop the timeout timer
        StopTimeoutTimer();
        
        Debug.Log("VR Button interaction detected on: " + buttonInteractable.gameObject.name);
        
        // Visual feedback - change button color to green
        Renderer buttonRenderer = buttonInteractable.GetComponent<Renderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = Color.green;
        }
        
        // Audio feedback
        AudioSource audio = buttonInteractable.GetComponent<AudioSource>();
        if (audio != null) 
        {
            audio.Play();
        }
        
        // First, enable the game object
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
            Debug.Log($"Enabled: {objectToEnable.name}");
        }
        else
        {
            Debug.LogWarning("Game object to enable is null!");
        }
        
        // Then hide the canvas after a short delay to allow audio/visual feedback
        StartCoroutine(HideCanvasAfterDelay(0.5f));
    }
    
    IEnumerator HideCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(false);
            Debug.Log($"Hidden canvas: {targetCanvas.name}");
        }
        else
        {
            Debug.LogWarning("Target canvas is null!");
        }
    }
    
    void RestartScene()
    {
        // Get the current active scene and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    void OnDestroy()
    {
        // Stop timeout timer when object is destroyed
        StopTimeoutTimer();
        
        // Clean up VR interaction listeners
        if (button1Interactable != null)
        {
            button1Interactable.selectEntered.RemoveAllListeners();
        }
        
        if (button2Interactable != null)
        {
            button2Interactable.selectEntered.RemoveAllListeners();
        }
    }
}