using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator animator;
    private CameraShake cameraShake;
    private AudioSource audioSource;     // Reference to the AudioSource component
    private bool isPlaying = false;
    private Turntable turntable;

    // Reference to the animation clip
    public AnimationClip thunderboltClip;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioSource = GetComponent<AudioSource>();
        turntable = Camera.main.GetComponent<Turntable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying)
        {
            turntable.enabled = false;
            animator.Play("Thunderbolt", 0, 0f);
            audioSource.Play();
            StartCoroutine(cameraShake.Shake(thunderboltClip.length, 0.01f));
            StartCoroutine(ResetAnimatorState(thunderboltClip.length));
            isPlaying = true;
        }
    }
    private IEnumerator ResetAnimatorState(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play("Idle");
        isPlaying = false;
        turntable.enabled = true;
    }
}