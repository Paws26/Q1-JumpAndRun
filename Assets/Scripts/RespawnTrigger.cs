using Unity.VisualScripting;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController playerController = other.gameObject.GetComponent<CharacterController>();
            if (playerController != null)
            {
                playerController.enabled = false; // Disable the CharacterController to avoid physics issues
                other.transform.position = respawnPoint.position; // Move the player to the respawn point
                playerController.enabled = true; // Re-enable the CharacterController
            }
        }
    }
}
