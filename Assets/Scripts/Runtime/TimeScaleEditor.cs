using UnityEngine;
using UnityEngine.InputSystem;

public class TimeScaleEditor : MonoBehaviour
{
    [SerializeField] private float timeScale = 1f;

    private InputAction timeScaleUpAction;
    private InputAction timeScaleDownAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.timeScaleUpAction = InputSystem.actions.FindAction("TimeScaleUp");
        this.timeScaleDownAction = InputSystem.actions.FindAction("TimeScaleDown");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.timeScaleUpAction.WasPressedThisFrame())
        {
            this.timeScale += 0.1f;
            Time.timeScale = this.timeScale;
            Debug.Log("Time Scale: " + Time.timeScale);
        }
        else if (this.timeScaleDownAction.WasPressedThisFrame())
        {
            this.timeScale -= 0.1f;
            Time.timeScale = this.timeScale;
            Debug.Log("Time Scale: " + Time.timeScale);
        }
    }
}
