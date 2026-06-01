
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

    public class Chest : MonoBehaviour {
    [SerializeField]
    private float openTime;
    [SerializeField]
    private Transform openPosition;
    [SerializeField]
    private Transform closedPosition;
    [SerializeField]
    private GameObject chestLid;
    [SerializeField]
    private GameObject winJewel;

    private bool open = false;
    private bool interpolating = false;
    private float currentInterpolationTime = 0.0f;
    private bool playerInRange = false;
    private InputAction interactAction;

    void Start() {
        this.interactAction = InputSystem.actions.FindAction("Interact");
    }

    void OpenChest() {
        this.open = true;
        this.StartCoroutine(this.InterpolateChestCoroutine());
    }

    void FixedUpdate()
    {
        if(this.interactAction.WasPressedThisFrame() && !this.interpolating && this.playerInRange && !this.open) {
            this.OpenChest();
        }
    }
    IEnumerator InterpolateChestCoroutine() {
        this.interpolating = true;
        Vector3 startPosition, targetPosition;
        Quaternion startRotation, targetRotation;
        startPosition = this.closedPosition.position;
        startRotation = this.closedPosition.rotation;
        targetPosition = this.openPosition.position;
        targetRotation = this.openPosition.rotation;
        this.winJewel.SetActive(true);
        this.currentInterpolationTime = 0.0f;
        while(this.currentInterpolationTime < this.openTime) {
            float percentage = this.currentInterpolationTime / this.openTime;
            var currentPosition = Vector3.Lerp(startPosition, targetPosition, percentage);
            var currentRotation = Quaternion.Slerp(startRotation, targetRotation, percentage);
            this.chestLid.transform.SetPositionAndRotation(currentPosition, currentRotation);
            yield return null;
            this.currentInterpolationTime += Time.deltaTime;
        }
        this.chestLid.transform.SetPositionAndRotation(targetPosition, targetRotation);
        this.open = true;
        this.interpolating = false;
    }

    // Trigger colliders to determine if the player is in range to interact with the chest.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag + " entered chest trigger.");
        if(other.CompareTag("Player")) {
            this.playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other.tag + " exited chest trigger.");
        if(other.CompareTag("Player")) {
            this.playerInRange = false;
        }
    }
}