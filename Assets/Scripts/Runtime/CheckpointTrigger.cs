using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] public Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            respawnPoint.position = transform.position;
        }
    }
}
