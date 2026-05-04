using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public class SawHandler : MonoBehaviour
{
    [Header("Saw Settings")]
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip cuttingSound;
    private bool isCutting = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.spatialBlend = 1f; // Make the sound 3D
    }

    void Start()
    {
        SetState(false);
        audioSource.clip = idleSound;
        audioSource.Play();
    }
    private void Update()
    {
        // Rotate the saw around its local z-axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetState(false);
        }
    }

    private void SetState(bool cutting)
    {
        if (isCutting == cutting) return;

        if (cutting)
        {
            isCutting = true;
            audioSource.clip = cuttingSound;
        }
        else
        {
            isCutting = false;
            audioSource.clip = idleSound;
        }

        audioSource.Play();
    }
}
