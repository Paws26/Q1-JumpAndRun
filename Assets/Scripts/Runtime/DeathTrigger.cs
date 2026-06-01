using Unity.VisualScripting;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponentInChildren<Character>();
        if (character != null)
        {
            character.InflictDamage(character.GetMaxHealth()); // Inflict damage over time
        }
        
        /*
        if (other.CompareTag("Player"))
        {
            CharacterController playerController = other.gameObject.GetComponent<CharacterController>();
            if (playerController != null)
            {
                playerController.enabled = false; // Disable the CharacterController to avoid physics issues
                other.transform.position = respawnPoint.position; // Move the player to the respawn point
                playerController.enabled = true; // Re-enable the CharacterController
            }
        }*/
    }
}
