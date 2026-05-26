using UnityEngine;

public class Jewel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.Win();
            Destroy(this.gameObject);
        }
    }
}
