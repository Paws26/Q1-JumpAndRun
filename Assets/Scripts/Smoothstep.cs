using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class SmoothstepTween : MonoBehaviour
{
    [SerializeField] private Vector3 a;
    [SerializeField] private Vector3 b;
    [SerializeField] private float speed;
    IEnumerator SmoothStepForward()
    {
        this.transform.position = this.a;
        float t = 0.0f;
        while(t < 1.0f)
        {
            float g = Mathf.SmoothStep(0.0f, 1.0f, t);
            this.transform.position = Vector3.Lerp(this.a, this.b, g);
            t += Time.deltaTime * this.speed;
            yield return null;
        }

        this.transform.position = this.b;
    }

    IEnumerator SmoothStepBackward()
    {
        this.transform.position = this.a;
        float t = 1.0f;
        while(t >= 0.0f)
        {
            float g = Mathf.SmoothStep(0.0f, 1.0f, t);
            this.transform.position = Vector3.Lerp(this.a, this.b, g);
            t -= Time.deltaTime * this.speed;
            yield return null;
        }

        this.transform.position = this.b;
    }

    IEnumerator Start()
    {
        yield return this.StartCoroutine(this.SmoothStepForward());
        yield return new WaitForSeconds(2.0f);
        yield return this.StartCoroutine(this.SmoothStepBackward());
    }
}