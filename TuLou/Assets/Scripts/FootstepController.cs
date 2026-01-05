using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FootstepController : MonoBehaviour
{
    [Header("脚步音效（单个循环）")]
    public AudioClip loopFootstepClip;

    private CharacterController controller;
    private AudioSource audioSource;

    private Vector3 lastPosition;
     
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (loopFootstepClip != null)
        {
            audioSource.clip = loopFootstepClip;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }

        lastPosition = transform.position;
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;
        bool isMoving = Vector3.Distance(transform.position, lastPosition) > 0.01f;

        if (isGrounded && isMoving)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }

        lastPosition = transform.position;
    }
}
