using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

    public class Lever : MonoBehaviour {
    [SerializeField]
    private float switchTime;
    [SerializeField]
    private Transform onPosition;
    [SerializeField]
    private Transform offPosition;
    [SerializeField]
    private GameObject leverHandle;
    
    private bool on = false;
    private bool interpolating = false;
    private float currentInterpolationTime = 0.0f;
    private bool playerInRange = false;
    private bool leverLocked = false;
    public UnityEvent<bool> onLeverToggled;
    private InputAction interactAction;
 
    void Start() {
        this.interactAction = InputSystem.actions.FindAction("Interact");
    }
    
    void ToggleLever() {
        this.on = !this.on;
        this.onLeverToggled.Invoke(this.on);
        this.StartCoroutine(this.InterpolateLeverCoroutine());
    }

    void FixedUpdate()
    {
        if(this.interactAction.WasPressedThisFrame() && !this.interpolating && this.playerInRange && !leverLocked) {
            this.ToggleLever();
        }
    }
    IEnumerator InterpolateLeverCoroutine() {
        this.interpolating = true;
        Vector3 startPosition, targetPosition;
        Quaternion startRotation, targetRotation;
        if(this.on) {
            startPosition = this.offPosition.position;
            startRotation = this.offPosition.rotation;
            targetPosition = this.onPosition.position;
            targetRotation = this.onPosition.rotation;
        } else {
            startPosition = this.onPosition.position;
            startRotation = this.onPosition.rotation;
            targetPosition = this.offPosition.position;
            targetRotation = this.offPosition.rotation;
        }
        this.currentInterpolationTime = 0.0f;
        while(this.currentInterpolationTime < this.switchTime) {
            float percentage = this.currentInterpolationTime / this.switchTime;
            var currentPosition = Vector3.Lerp(startPosition, targetPosition, percentage);
            var currentRotation = Quaternion.Slerp(startRotation, targetRotation, percentage);
            this.leverHandle.transform.SetPositionAndRotation(currentPosition, currentRotation);
            yield return null;
            this.currentInterpolationTime += Time.deltaTime;
        }
        this.leverHandle.transform.SetPositionAndRotation(targetPosition, targetRotation);
        this.interpolating = false;
    }

    // Trigger colliders to determine if the player is in range to interact with the lever.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag + " entered lever trigger.");
        if(other.CompareTag("Player")) {
            this.playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other.tag + " exited lever trigger.");
        if(other.CompareTag("Player")) {
            this.playerInRange = false;
        }
    }

    public void SetLeverLocked(bool locked) {
        this.leverLocked = locked;
    }
}