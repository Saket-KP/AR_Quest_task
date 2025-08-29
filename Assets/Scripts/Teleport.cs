
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Teleport : MonoBehaviour
{
    public GameObject reference;  // Where the player (XR Origin) will be teleported to
    public GameObject player;    // Assign the XR Origin GameObject here

    private CharacterController characterController; // For XR Origin movement

    void Start()
    {
        // Get the CharacterController from the XR Origin (if it has one)
        characterController = player.GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogWarning("CharacterController not found on XR Origin. Teleport may not work smoothly.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        SceneManager.LoadScene("Upper OAT");

            // Optional: Add any additional logic here
        }
    }

  
