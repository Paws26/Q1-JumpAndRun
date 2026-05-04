using UnityEngine;
using UnityEngine.Audio;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    private void Update()
    {
        // Rotate the coin around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        var collectSoundObject = new GameObject("CoinCollectSound");
        collectSoundObject.transform.position = transform.position;
        var audioSource = collectSoundObject.AddComponent<AudioSource>();
        audioSource.clip = collectSound;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.Play();
        Destroy(collectSoundObject, collectSound.length);

        Destroy(this.gameObject);
    }
}
