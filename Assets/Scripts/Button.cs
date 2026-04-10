using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{   
    [SerializeField] private Transform pressedPosition;
    [SerializeField] private Transform unpressedPosition;
    [SerializeField] private float pressTime;
    private GameObject button;
    public UnityEvent onButtonPressed; // Event to notify when the button is pressed

    private bool isPressed = false;
    private bool interpolating = false;
    private float currentInterpolationTime = 0.0f;
    void Start() {
        this.button = this.transform.GetChild(0).gameObject; // Assuming the button is the first child of the GameObject this script is attached to
    }

    IEnumerator InterpolateButtonCoroutine() {
        this.interpolating = true;
        Vector3 startPosition, targetPosition;
        Quaternion startRotation, targetRotation;
        if(this.isPressed) {
            startPosition = this.unpressedPosition.position;
            startRotation = this.unpressedPosition.rotation;
            targetPosition = this.pressedPosition.position;
            targetRotation = this.pressedPosition.rotation;
        } else {
            startPosition = this.pressedPosition.position;
            startRotation = this.pressedPosition.rotation;
            targetPosition = this.unpressedPosition.position;
            targetRotation = this.unpressedPosition.rotation;
        }
        this.currentInterpolationTime = 0.0f;
        while(this.currentInterpolationTime < this.pressTime) {
            float percentage = this.currentInterpolationTime / this.pressTime;
            var currentPosition = Vector3.Lerp(startPosition, targetPosition, percentage);
            var currentRotation = Quaternion.Slerp(startRotation, targetRotation, percentage);
            this.button.transform.SetPositionAndRotation(currentPosition, currentRotation);
            yield return null; // Wait for the next frame
            this.currentInterpolationTime += Time.deltaTime; // Increment the interpolation time
        }
        // Ensure the button is exactly at the target position and rotation at the end of the interpolation
        this.button.transform.SetPositionAndRotation(targetPosition, targetRotation);
        this.interpolating = false; // Mark interpolation as complete
    }

    // Detect when the player steps on the button
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !this.interpolating && !this.isPressed) {
            this.isPressed = true;
            this.onButtonPressed.Invoke(); // Notify listeners about the button press
            this.StartCoroutine(this.InterpolateButtonCoroutine());
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && !this.interpolating && this.isPressed) {
            this.isPressed = false;
            this.StartCoroutine(this.InterpolateButtonCoroutine());
        }
    }
}
