using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class destroyObjects : MonoBehaviour 
{
    [Header("Destruction Counter")]
    public int destructionCount = 0;
    public int targetCount = 1;
    
    [Header("Reward Canvas")]
    public Canvas rewardCanvas;
    public float canvasDisplayTime = 10f;
    
    // Track objects that are already being processed for destruction
    private HashSet<GameObject> objectsBeingDestroyed = new HashSet<GameObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure the reward canvas is initially hidden
        if (rewardCanvas != null)
        {
            rewardCanvas.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when another collider enters the trigger zone
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered has the "plastic" tag
        if (other.gameObject.CompareTag("Plastic"))
        {
            // Get the parent object
            GameObject parentObject = other.transform.parent?.gameObject;
            GameObject objectToDestroy;
            string logMessage;
            
            if (parentObject != null)
            {
                objectToDestroy = parentObject;
                logMessage = "Destroyed plastic object's parent: " + parentObject.name;
            }
            else
            {
                objectToDestroy = other.gameObject;
                logMessage = "Destroyed plastic object (no parent): " + other.gameObject.name;
            }
            
            // Check if this object is already being processed for destruction
            if (objectsBeingDestroyed.Contains(objectToDestroy))
            {
                Debug.Log("Object " + objectToDestroy.name + " already being destroyed, skipping...");
                return;
            }
            
            // Check if object is still active (not already destroyed)
            if (!objectToDestroy.activeInHierarchy)
            {
                Debug.Log("Object " + objectToDestroy.name + " is already inactive, skipping...");
                return;
            }
            
            // Mark object as being destroyed
            objectsBeingDestroyed.Add(objectToDestroy);
            
            // Increment the destruction count
            destructionCount++;
            
            // Log the destruction with object name and current count
            Debug.Log(logMessage + " | Count: " + destructionCount + " | Child that triggered: " + other.gameObject.name);
            
            // Destroy the target object
            Destroy(objectToDestroy);
            
            // Remove from tracking after a short delay (cleanup)
            StartCoroutine(RemoveFromTracking(objectToDestroy, 0.1f));
            
            if (destructionCount >= targetCount)
            {
                OnTargetCountReached();
            }
        }
    }
    
    IEnumerator RemoveFromTracking(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        objectsBeingDestroyed.Remove(obj);
    }
    
    void OnTargetCountReached()
    {
        Debug.Log("Target count of " + targetCount + " reached! Executing special function...");
        TriggerReward();
    }
    
    void TriggerReward()
    {
        Debug.Log("Reward triggered!");
        
        if (rewardCanvas != null)
        {
            StartCoroutine(ShowCanvasTemporarily());
        }
        else
        {
            Debug.LogWarning("Reward canvas is not assigned!");
        }
    }
    
    IEnumerator ShowCanvasTemporarily()
    {
        // Enable the canvas
        rewardCanvas.gameObject.SetActive(true);
        Debug.Log("Reward canvas enabled");
        
        // Find the CanvasButtonController and start its timer
        CanvasButtonController buttonController = rewardCanvas.GetComponent<CanvasButtonController>();
        if (buttonController == null)
        {
            buttonController = rewardCanvas.GetComponentInChildren<CanvasButtonController>();
        }
        
        if (buttonController != null)
        {
            // The timer will start automatically via OnEnable in CanvasButtonController
            Debug.Log("CanvasButtonController found - timer will start automatically");
        }
        else
        {
            Debug.LogWarning("CanvasButtonController not found on reward canvas!");
            
            // Fallback: disable canvas after the display time if no controller found
            yield return new WaitForSeconds(canvasDisplayTime);
            rewardCanvas.gameObject.SetActive(false);
            Debug.Log("Reward canvas disabled (fallback)");
        }
    }
}