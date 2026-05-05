using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class EnemySquish : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject enemyParent; 

    // Squish enemy using DOTween
    [Header("Squish Settings")]
    public float squishDuration = 0.3f;
    [SerializeField]
    private Vector3 squishScale = new Vector3(1f, 0.1f, 1f);

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioClip squishSound;
    [SerializeField] [Range(0f, 1f)] private float squishVolume;

    private Sequence squishSequence;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.spatialBlend = 1f; // Make the sound 3D
    }

    void Start()
    {
        audioSource.clip = squishSound;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = squishVolume; // Randomize volume for variety
    }

    private void CreateSequence() {
        squishSequence = DOTween.Sequence();
        audioSource.Play();
        squishSequence.Append(enemyParent.transform.DOScale(squishScale, squishDuration).SetEase(Ease.InOutQuad));
        squishSequence.Append(enemyParent.transform.DOScale(Vector3.zero, squishDuration/2f).SetEase(Ease.InOutQuad));
        squishSequence.OnComplete(() => Destroy(enemyParent));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (squishSequence == null || !squishSequence.IsPlaying()) {
                // Disable enemy ai
                enemyParent.GetComponent<EnemyAI>().Die();
                enemyParent.GetComponent<EnemyAI>().enabled = false;
                this.CreateSequence();
                squishSequence.Play();
            }
        }
    } 
}
